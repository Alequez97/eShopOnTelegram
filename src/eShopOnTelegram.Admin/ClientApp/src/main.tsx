import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App';
import axios from 'axios';
import './reset.css';

axios.defaults.baseURL = import.meta.env.VITE_BACKEND_API_BASE_URL ?? '/api';

ReactDOM.createRoot(document.getElementById('root') as HTMLElement).render(
	<React.StrictMode>
		<App />
	</React.StrictMode>,
);
