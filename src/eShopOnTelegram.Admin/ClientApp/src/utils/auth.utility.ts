import axios, { AxiosError } from 'axios';
import {
	ACCESS_TOKEN_LOCAL_STORAGE_KEY,
	LoginRequest,
	LoginResponse,
	REFRESH_TOKEN_LOCAL_STORAGE_KEY,
} from '../types/auth.type';

export const login = async (request: LoginRequest) => {
	const response = await axios.post('/auth/login', request);

	const auth = response.data as LoginResponse;
	localStorage.setItem(ACCESS_TOKEN_LOCAL_STORAGE_KEY, auth.accessToken);
	localStorage.setItem(REFRESH_TOKEN_LOCAL_STORAGE_KEY, auth.refreshToken);

	return auth;
};

export const refreshAccessToken = async () => {
	const refreshToken = localStorage.getItem(REFRESH_TOKEN_LOCAL_STORAGE_KEY);

	if (!refreshToken) {
		throw new Error('Does not have refresh token');
	}

	try {
		const response = await axios.post('/auth/token/refresh', {
			refreshToken,
		});

		const newAccessToken = response.data.accessToken;
		const newRefreshToken = response.data.refreshToken;
		localStorage.setItem(ACCESS_TOKEN_LOCAL_STORAGE_KEY, newAccessToken);
		localStorage.setItem(REFRESH_TOKEN_LOCAL_STORAGE_KEY, newRefreshToken);

		return newAccessToken;
	} catch (error: unknown) {
		if (
			error instanceof AxiosError &&
			error.response &&
			error.response.status === 400
		) {
			// Refresh token expired
			localStorage.removeItem(ACCESS_TOKEN_LOCAL_STORAGE_KEY);
			localStorage.removeItem(REFRESH_TOKEN_LOCAL_STORAGE_KEY);
			return;
		}

		throw error;
	}
};
