import React from 'react';
import ReactDOM from 'react-dom/client';
import './main.css';
import App from './App';
import axios from 'axios';

axios.defaults.baseURL = import.meta.env.VITE_BACKEND_API_BASE_URL ?? '/api';

const root = ReactDOM.createRoot(
	document.getElementById('root') as HTMLElement,
);
root.render(
	<React.StrictMode>
		<App />
	</React.StrictMode>,
);
