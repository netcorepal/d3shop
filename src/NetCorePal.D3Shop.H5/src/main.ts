import { createApp } from 'vue';
import { createPinia } from 'pinia';
import App from './App.vue';
import router from './router';
import { setupAuth } from './directives/auth';
import { useAuthStore } from '@/store/auth';
import { useAppStore } from '@/store/app';
import i18n from './i18n';
import { setupComponents } from './utils/initComponents';
import 'vant/lib/index.css';
// import { useAppStore } from './store/app';

const app = createApp(App);
const pinia = createPinia();
  
// 初始化pinia
app.use(pinia);

// 初始化路由
app.use(router);

// 初始化国际化
app.use(i18n);

// 初始化组件
setupComponents(app);

// 初始化权限
const authStore = useAuthStore();
setupAuth(app, router, authStore);

// 初始化主题变量
const appStore = useAppStore();
appStore.initThemeVars();

app.mount('#app');
