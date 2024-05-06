import { defineConfig } from 'vite';
import mkcert from 'vite-plugin-mkcert';
import react from '@vitejs/plugin-react';
import path from 'path';

// https://vitejs.dev/config/
export default defineConfig({
	publicDir: './public',
	resolve: {
		alias: {
			'@': path.resolve(__dirname, './src'),
		},
	},
	plugins: [react(), mkcert()],
	build: {
		outDir: 'build',
		sourcemap: false,
		minify: true,
	},
	server: {
		port: 3500,
		strictPort: true,
		proxy: {
			'/api': {
				target: 'https://localhost:7224',
				changeOrigin: true,
				secure: false,
				rewrite: (path) => path.replace(/^\/api/, '/api'),
			},
		},
	},
});
