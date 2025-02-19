import { createApp } from 'vue'
import App from './App.vue'

import router from './router'

// vant ui
import vant from 'vant';

const app = createApp(App);
app.use(router);
app.use(vant);
app.mount('#app')
