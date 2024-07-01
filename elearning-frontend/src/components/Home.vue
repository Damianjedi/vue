<template>
  <div>
    <h1>Posts</h1>
    <form @submit.prevent="createPost">
      <textarea v-model="newPostContent" placeholder="Write a new post..."></textarea>
      <button type="submit">Add Post</button>
    </form>
    <div v-if="error" class="error">{{ error }}</div>
    <div v-for="post in posts" :key="post.id" class="post">
      <p>{{ post.content }}</p>
      <small>Posted by {{ post.user.firstName }} on {{ post.createdAt }}</small>
      <form @submit.prevent="createComment(post.id)">
        <textarea v-model="newCommentContent[post.id]" placeholder="Write a comment..."></textarea>
        <button type="submit">Add Comment</button>
      </form>
      <div v-for="comment in post.comments" :key="comment.id" class="comment">
        <p>{{ comment.content }}</p>
        <small>Commented by {{ comment.user.firstName }} on {{ comment.createdAt }}</small>
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name:'HomePage',
  data() {
    return {
      posts: [],
      newPostContent: '',
      newCommentContent: {},
      error: ''
    };
  },
  async created() {
    await this.fetchPosts();
  },
  methods: {
    async fetchPosts() {
      try {
        const response = await axios.get('https://localhost:7153/api/elearning/posts');
        this.posts = response.data;
      } catch (error) {
        this.error = 'Error fetching posts';
        console.error(error);
      }
    },
    async createPost() {
      try {
        const token = localStorage.getItem('token');
        if (!token) {
          this.error = 'You must be logged in to create a post';
          return;
        }
        const response = await axios.post('https://localhost:7153/api/elearning/posts', 
          {Content: this.newPostContent }, 
          {headers: {Authorization: `Bearer ${token}` } }
          );
        this.posts.push(response.data);
        this.newPostContent = '';
      } catch (error) {
        this.error = error.response ? error.response.data : 'Error creating post';
        console.error(error);
      }
    },
    async createComment(postId) {
      try {
        const token = localStorage.getItem('token');
        if (!token) {
          this.error = 'You must be logged in to comment';
          return;
        }
        const response = await axios.post(`https://localhost:7153/api/elearning/posts/${postId}/comments`, 
          { content: this.newCommentContent[postId] }, 
          { headers: { Authorization: `Bearer ${token}` } }
        );
        const post = this.posts.find(p => p.id === postId);
        post.comments.push(response.data);
        this.$set(this.newCommentContent, postId, '');
      } catch (error) {
        this.error = error.response ? error.response.data : 'Error creating comment';
        console.error(error);
      }
    }
  }
};
</script>

<style>
.post {
  border: 1px solid #ccc;
  padding: 1rem;
  margin: 1rem 0;
}

.error {
  color: red;
}
</style>
