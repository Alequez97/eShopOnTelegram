import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vitejs.dev/config/
export default defineConfig({
  publicDir: "./public",
  plugins: [react()],
  css: {
    modules:{
      localsConvention: 'camelCaseOnly'
    }
  },
  build: {
    outDir: "build",
    sourcemap: true,
    minify: true,
  },
})
