<template>
  <div class="login">
    <h2>Login</h2>
    <form @submit.prevent="login">
      <div>
        <label for="email">Email</label>
        <input type="email" v-model="email" required />
      </div>
      <div>
        <label for="password">Password</label>
        <input type="password" v-model="password" required />
      </div>
      <button type="submit">Login</button>
    </form>
    <p>{{ error }}</p>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name: 'UserLogin',
  data() {
    return {
      email: '',
      password: '',
      error: ''
    };
  },
  methods: {
    async login() {
      try {
        const response = await axios.post('https://localhost:7153/api/elearning/login', {
          email: this.email,
          password: this.password
        });
        localStorage.setItem('token', response.data.token);
        this.$router.push('/');
      } catch (error) {
        this.error = 'Invalid login credentials';
      }
    }
  }
};
</script>

<style>

.login {
  max-width: 600px;
  margin: 0 auto;
  border-radius: 15px;
  padding: 20px;
  background-color: white;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
  margin: 100px auto auto auto;
}

.login div {
  resize: none;
  margin: 10px auto auto auto;
}

label {
    display: block;
    margin-bottom: 5px;
    text-align: left;
    box-sizing: border-box;
    width: 100%;
    font-family: Kanit;
    font-weight: 200;
    text-align: center;
  }

  button {
    margin: 25px auto;
  }


</style>
