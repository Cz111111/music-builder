import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [vue()],
  server: {
    proxy: {
      // 捕获所有以 /api 开头的请求
      '/api': {
        // 目标是后端服务器地址
        target: 'https://127.0.0.1:7129',
        // 重写请求路径，去掉 /api 前缀
        
        changeOrigin: true, // 改变原始请求的origin头为 target URL 的域名
        secure: false, // 如果是https的代理，需要设置secure为true
      },
      // 如果有更多代理规则，可以继续添加
    },
  },
})
