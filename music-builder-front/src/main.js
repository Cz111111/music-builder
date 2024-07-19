import { createApp } from 'vue'
import { createPinia } from 'pinia'

import ElementPlus from 'element-plus'
import 'element-plus/dist/index.css'
import './assets/global.css'

import App from './App.vue'
import router from './router/index.js'
import * as ElementPlusIconsVue from '@element-plus/icons-vue'
import axios from 'axios'

axios.interceptors.request.use(config => {
  console.log('Request URL:', config.url);
  return config;
});
const app = createApp(App)
for (const [key, component] of Object.entries(ElementPlusIconsVue)) {
    app.component(key, component)
  }
const pinia = createPinia();
app.use(ElementPlus)
app.use(pinia)
app.use(router)

app.mount('#app')
