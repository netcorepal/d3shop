import { createApp } from 'vue';
import { createPinia } from 'pinia';
import App from './App.vue';
import router from './router';
import { setupAuth } from './directives/auth';
import { useAuthStore } from '@/store/auth';
import { useAppStore } from '@/store/app';
import i18n from './i18n';
// import { useAppStore } from './store/app';
import './styles/index.css';

const app = createApp(App);
const pinia = createPinia();
app.use(pinia);
app.use(router);
app.use(i18n);

const authStore = useAuthStore();
setupAuth(app, router, authStore);

// 初始化主题变量
const appStore = useAppStore();
appStore.initThemeVars();

app.mount('#app');
