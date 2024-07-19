import { fileURLToPath, URL } from 'node:url'

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import vueJsx from '@vitejs/plugin-vue-jsx'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [
    vue(),
    vueJsx(),
  ],
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    }
  }
  ,
  server:{
    host:"172.20.10.12",
    port:"9000",
    proxy:{
      '/api/ksong': {
        // 目标是后端服务器地址
        target: 'https://127.0.0.1:7129',
        // 重写请求路径，去掉 /api 前缀
        
        changeOrigin: true, // 改变原始请求的origin头为 target URL 的域名
        secure: false, // 如果是https的代理，需要设置secure为true
      },
      '/api/chat': {
        // 目标是后端服务器地址
        target: 'http://172.20.10.4:9999',
        // 重写请求路径，去掉 /api 前缀
        
        secure: false, // 如果是https的代理，需要设置secure为true
        rewrite:(path)=>path.replace(/^\/api/,'')
      },
      '/api':{
          target:'http://localhost:8080',
          changeOrigin:true,
          secure:false,
          rewrite:(path)=>path.replace(/^\/api/,'')
      }
 
    }
  }
})
