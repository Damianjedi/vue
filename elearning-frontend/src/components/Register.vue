<template>
    <div class="register">
      <h2>Register</h2>
      <form @submit.prevent="register">
        <div>
          <label for="firstName">First Name</label>
          <input type="text" v-model="firstName" required />
        </div>
        <div>
          <label for="lastName">Last Name</label>
          <input type="text" v-model="lastName" required />
        </div>
        <div>
          <label for="email">Email</label>
          <input type="email" v-model="email" required />
        </div>
        <div>
          <label for="password">Password</label>
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
  .register {
  max-width: 600px;
  margin: 0 auto;
  border-radius: 15px;
  padding: 20px;
  background-color: white;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
  margin: 100px auto auto auto;
}

.register div {
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
  