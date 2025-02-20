import { createApp } from 'vue';
import { createPinia } from 'pinia';
import App from './App.vue';
import router from '@/router';
import { setupAuth } from './directives/auth';
import { useAuthStore } from '@/store/auth';

const app = createApp(App);
const pinia = createPinia();
app.use(pinia);
app.use(router);

const authStore = useAuthStore();
setupAuth(app, router, authStore);

app.mount('#app');
