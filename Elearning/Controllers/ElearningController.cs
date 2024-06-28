using Elearning.Data;
using Elearning.Models;
using ElearningApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Elearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElearningController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;


        public ElearningController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUser model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("User created successfully!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            if (result.Succeeded)
            {
                var appUser = await _userManager.FindByEmailAsync(model.Email);
                var token = GenerateJwtToken(appUser);
                return Ok(new { token });
            }

            return Unauthorized();
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        

            

            [HttpGet("posts")]
            public async Task<IActionResult> GetPosts()
            {
                var posts = await _context.Posts.Include(p => p.User).Include(p => p.Comments).ToListAsync();
                return Ok(posts);
            }

        [HttpPost("posts")]
        [Authorize]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var newPost = new Post
            {
                UserId = user.Id,
                Content = model.Content,
                CreatedAt = DateTime.Now
            };

            _context.Posts.Add(newPost);
            await _context.SaveChangesAsync();
            return Ok(newPost);
        }

        [HttpPost("posts/{postId}/comments")]
            [Authorize]
            public async Task<IActionResult> AddComment(int postId, [FromBody] Comment comment)
            {
                var user = await _userManager.GetUserAsync(User);
                comment.UserId = user.Id;
                comment.PostId = postId;
                comment.CreatedAt = DateTime.Now;
                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();
                return Ok(comment);
            }
        

        

            [HttpGet("chat/{userId}")]
            [Authorize]
            public async Task<IActionResult> GetMessages(string userId)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                var messages = await _context.ChatMessages
                    .Where(m => (m.SenderId == currentUser.Id && m.ReceiverId == userId) ||
                                (m.SenderId == userId && m.ReceiverId == currentUser.Id))
                    .OrderBy(m => m.CreatedAt)
                    .ToListAsync();
                return Ok(messages);
            }

            [HttpPost("chat")]
            [Authorize]
            public async Task<IActionResult> SendMessage([FromBody] ChatMessage message)
            {
                var sender = await _userManager.GetUserAsync(User);
                message.SenderId = sender.Id;
                message.CreatedAt = DateTime.Now;
                _context.ChatMessages.Add(message);
                await _context.SaveChangesAsync();
                return Ok(message);
            }
        

        public class RegisterModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public class LoginModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
        public class CreatePostModel
        {
            public string Content { get; set; }
        }

        public class CreateCommentModel
        {
            public string Content { get; set; }
        }

        public class CreateMessageModel
        {
            public string ReceiverId { get; set; }
            public string Content { get; set; }
        }
    }
}
