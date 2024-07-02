<template>
  <div class="mainSection">
    <h1>Posty</h1>
    <form class="postPublish" @submit.prevent="createPost">
      <textarea v-model="newPostContent" placeholder="Napisz post..."></textarea>
      <button type="submit">Opublikuj</button>
    </form>
    <div v-if="error" class="error">{{ error }}</div>
    <div v-for="post in posts" :key="post.id" class="post">
      <p>{{ post.content }}</p>
      <small>Opublikowano przez {{ post.user.firstName }} dnia {{ formatDate(post.createdAt) }}</small>
      <form class="commentPublish" @submit.prevent="createComment(post.id)">
        <textarea v-model="newCommentContent[post.id]" placeholder="Napisz komentarz..."></textarea>
        <button type="submit">Opublikuj</button>
      </form>
      <div v-for="comment in post.comments" :key="comment.id" class="comment">
        <p>{{ comment.content }}</p>
        <small>Skomentowano przez {{ comment.user.firstName }} dnia {{ formatDate(comment.createdAt) }}</small>
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
      } catch (error) {
        this.error = error.response ? error.response.data : 'Error creating comment';
        console.error(error);
      }
    }
  }
};
</script>

<!-- style css -->

<style>


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
  margin: 20px auto auto auto;
}

.commentPublish {
  margin: 20px auto auto auto;
}

textarea {
  width: 100%;
  max-width: 300px;
  padding: 10px;
  margin-bottom: 10px;
  border: 1px solid #ddd;
  border-radius: 5px;
  resize: none;
  margin: 20px auto auto auto;
}

.comment-form {
  display: flex;
  flex-direction: column;
  margin-bottom: 20px;
}

button {
  align-self: flex-end;
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
  margin-bottom: 20px;
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