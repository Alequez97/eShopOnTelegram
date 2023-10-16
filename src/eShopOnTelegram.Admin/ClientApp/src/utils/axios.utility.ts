import axios, { AxiosError } from 'axios';
import {
	ACCESS_TOKEN_LOCAL_STORAGE_KEY,
	REFRESH_TOKEN_LOCAL_STORAGE_KEY,
} from '../types/auth.type';
import { refreshAccessToken } from './auth.utility';

export const axiosGet = async (url: string) => {
	return axiosRequestWithRefreshToken('get', url);
};

export const axiosPost = async <T>(url: string, request: T) => {
	return axiosRequestWithRefreshToken('post', url, request);
};

export const axiosPatch = async <T>(url: string, request: T) => {
	return axiosRequestWithRefreshToken('patch', url, request);
};

const axiosRequestWithRefreshToken = async <T>(
	method: string,
	url: string,
	request?: T,
) => {
	try {
		const accessToken = localStorage.getItem(
			ACCESS_TOKEN_LOCAL_STORAGE_KEY,
		);

		const config = {
			method,
			url,
			headers: { Authorization: `Bearer ${accessToken}` },
			data: request,
		};

		const { data } = await axios(config);

		return data;
	} catch (error: unknown) {
		if (
			error instanceof AxiosError &&
			error.response &&
			error.response.status === 401
		) {
			const refreshToken = localStorage.getItem(
				REFRESH_TOKEN_LOCAL_STORAGE_KEY,
			);
			if (refreshToken) {
				const newAccessToken = await refreshAccessToken(refreshToken);

				const config = {
					method,
					url,
					headers: { Authorization: `Bearer ${newAccessToken}` },
					data: request,
				};

				const { data } = await axios(config);

				return data;
			}
		}

		throw error;
	}
};
