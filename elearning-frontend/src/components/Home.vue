<template>
  <div class="mainSection">
    <h1>Posty</h1>
    <form class="postPublish" @submit.prevent="createPost">
      <textarea v-model="newPostContent" placeholder="Napisz post..."></textarea>
      <input type="file" id="fileInput" />
      <button type="submit">Opublikuj</button>
    </form>
    <div v-if="error" class="error">{{ error }}</div>
    <div v-for="post in posts" :key="post.id" class="post">
      <p>{{ post.content }}</p>
      <small>Opublikowano przez <span style="font-weight: bold"> {{ post.user.firstName }} </span> dnia {{ formatDate(post.createdAt) }}</small>
      <div v-if="post.fileName">
        <div class="holdImg" v-if="isImageFile(post.fileName)">
          <img :src="`https://localhost:7153/api/elearning/posts/${post.id}/file`" :alt="post.fileName" />
        </div>
        <div v-else>
          <a :href="`https://localhost:7153/api/elearning/posts/${post.id}/file`" target="_blank">{{ post.fileName }}</a>
        </div>
      </div>
      <form class="commentPublish" @submit.prevent="createComment(post.id)">
        <textarea v-model="newCommentContent[post.id]" placeholder="Napisz komentarz..."></textarea>
        <button type="submit">Opublikuj</button>
      </form>
      <div v-for="comment in post.comments" :key="comment.id" class="comment">
        <p>{{ comment.content }}</p>
        <small>Skomentowano przez <span style="font-weight: bold"> {{ comment.user.firstName }} </span> dnia {{ formatDate(comment.createdAt) }}</small>
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios';
import { format } from 'date-fns';

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
    formatDate(date) {
      return format(new Date(date), ' HH:mm dd.MM.yyyy')
    },
    async fetchPosts() {
      try {
        const response = await axios.get('https://localhost:7153/api/elearning/posts');
        this.posts = response.data.map(post => ({
          ...post,
          fileName: post.fileName || null
        }));
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
        const gg = ('FileContentType', '', 'FileName', '', 'FileData', new Blob());
        const formData = new FormData();
        formData.append('Content', this.newPostContent);
        const fileInput = document.getElementById('fileInput'); 
        if (fileInput.files.length > 0) {
        formData.append('File', fileInput.files[0]);
        } else {
            
            formData.append('File', gg);
        }
        const response = await axios.post('https://localhost:7153/api/elearning/posts', 
          formData,
          {headers: {Authorization: `Bearer ${token}`, 
            'Content-Type': 'multipart/form-data' } }
          );
          console.log(response.data);
        this.posts.push(response.data);
        this.newPostContent = '';
        window.location.reload()
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
        window.location.reload()
      } catch (error) {
        this.error = error.response ? error.response.data : 'Error creating comment';
        console.error(error);
        window.location.reload()
      }
    },
    isImageFile(fileName) {
      const extension = fileName.split('.').pop().toLowerCase();
      return ['jpg', 'jpeg', 'png'].includes(extension);
    },
  }
};
</script>

<!-- style css -->

<style>

.holdImg img {
  width: 400px;
  height: 600px;
  max-width: 100%;
  max-height: 100%;
}

.mainSection {
  max-width: 600px;
  margin: 0 auto;
  border-radius: 15px;
  padding: 20px;
  background-color: white;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
  margin: 100px auto auto auto;
}

.header {
  text-align: center;
  margin-bottom: 20px;
}

.post-form {
  display: flex;
  flex-direction: column;
  margin-bottom: 20px;
}

.postPublish {
  display: flex;
  flex-direction: column;
  align-items: center;
  margin: 20px auto auto auto;
}

.commentPublish {
  display: flex;
  flex-direction: column;
  align-items: center;
  margin: 20px auto auto auto;
}

#fileInput {
  margin: 10px auto;
}

textarea {
  width: 100%;
  max-width: 300px;
  padding: 10px;
  margin-bottom: 10px;
  border: 1px solid #ddd;
  border-radius: 5px;
  resize: none; 
}

.comment-form {
  display: flex;
  flex-direction: column;
  margin-bottom: 20px;
}

button {
  padding: 10px 20px;
  border-radius: 5px;
  background-color: #0047AB;
  color: white;
  border: none;
  cursor: pointer;
  transition: background-color 0.3s;
}

button:hover {
  background-color: #6495ED;
}

.error {
  color: red;
  margin-bottom: 20px;
  text-align: center;
}

.post {
  background-color: white;
  padding: 20px;
  border-radius: 10px;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
  margin: 30px auto;
}

.post-content {
  font-size: 15px;
  margin-bottom: 10px;
}

.post-meta {
  font-size: 12px;
  color: lightgray;
}

.comment {
  background-color: white;
  padding: 10px;
  border-radius: 10px;
  margin-top: 10px;
}

.comment-content {
  font-size: 14px;
  margin-bottom: 5px;
}

.comment-meta {
  font-size: 12px;
  color: lightgray;
}

</style>