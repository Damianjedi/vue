<template>
  <div id="app">
    <nav id="navbar">
      <router-link to="/" class="left"><img src="@/assets/EL.png" alt="Home" class="logo" /></router-link>
      <div class="right">
        <router-link to="/login">Login</router-link>
        <router-link to="/register">Register</router-link>
      </div>
    </nav>
    <router-view></router-view>
  </div>
</template>

<script>
export default {
  name: 'App'
};
</script>


<style>
.logo {
  height: 35px;
}
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
  margin: 10px 0 auto auto;
}
#navbar {
  display: flex;
  justify-content: space-between;
  align-items: center;
  background-color: #e5ecf1;
  width: 100%;
  height: 50px;
  box-shadow: 0 2px 4px rgba(34, 124, 228, 0.1);
  border-radius: 10px;
  margin: 10px 0 auto auto;
}

#navbar a {
  color: rgb(0, 89, 255);
  text-decoration: none;
  padding: 0.5rem 1rem;
}

body {
  background-color: #9ccaf5;
}
</style>
