<template>
    <div class="register">
      <h2>Register</h2>
      <form @submit.prevent="register">
        <div>
          <label for="firstName">First Name:</label>
          <input type="text" v-model="firstName" required />
        </div>
        <div>
          <label for="lastName">Last Name:</label>
          <input type="text" v-model="lastName" required />
        </div>
        <div>
          <label for="email">Email:</label>
          <input type="email" v-model="email" required />
        </div>
        <div>
          <label for="password">Password:</label>
          <input type="password" v-model="password" required />
        </div>
        <button type="submit" >Register</button>
      </form>
      <p>{{ error }}</p>
    </div>
  </template>
  
  <script>
  import axios from 'axios';
  
  export default {
    name: 'UserRegister',
    data() {
      return {
        firstName: '',
        lastName: '',
        email: '',
        password: '',
        error: ''
      };
    },
    methods: {
      async register() {
        try {
          await axios.post('https://localhost:7153/api/elearning/register', {
            firstName: this.firstName,
            lastName: this.lastName,
            email: this.email,
            password: this.password
          });
          this.$router.push('/login');
        } catch (error) {
          this.error = 'Failed to register';
        }
      }
    }
  };
  </script>
  
  <style>
  /* Add your styles here */
  </style>
  