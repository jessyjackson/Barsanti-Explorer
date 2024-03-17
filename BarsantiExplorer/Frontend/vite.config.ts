import {defineConfig} from 'vite'
import react from '@vitejs/plugin-react'

// https://vitejs.dev/config/
export default defineConfig({
    plugins: [react()],
    server: {
        port: 3000,
        proxy: {
            "/api": "http://localhost:5202/api",
            "/auth": "http://localhost:5202/auth",
        }
    },
    build: {
      outDir: "../wwwroot"  
    }
})