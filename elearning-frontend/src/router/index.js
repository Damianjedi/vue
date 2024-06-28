import { createRouter, createWebHistory } from 'vue-router';
import HomePage from '../components/Home.vue';
import UserLogin from '../components/Login.vue';
import UserRegister from '../components/Register.vue';

const routes = [
  { path: '/', component: HomePage, meta: { requiresAuth: true } },
  { path: '/login', component: UserLogin },
  { path: '/register', component: UserRegister }
];

const router = createRouter({
  history: createWebHistory(),
  routes
});

router.beforeEach((to, from, next) => {
  const requiresAuth = to.matched.some(record => record.meta.requiresAuth);
  const isAuthenticated = !!localStorage.getItem('token');
  
  if (requiresAuth && !isAuthenticated) {
    next('/login');
  } else {
    next();
  }
});

export default router;
