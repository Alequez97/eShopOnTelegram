/* eslint-disable */

import axios from 'axios';
import {
	ACCESS_TOKEN_LOCAL_STORAGE_KEY,
	REFRESH_TOKEN_LOCAL_STORAGE_KEY,
} from '../types/auth.type';
import { refreshAccessToken } from './auth.utility';

export const axiosGet = async (url: string) => {
	try {
		const accessToken = localStorage.getItem(
			ACCESS_TOKEN_LOCAL_STORAGE_KEY,
		);

		const { data } = await axios.get(url, {
			headers: { Authorization: `Bearer ${accessToken}` },
		});

		return data;
	} catch (error: any) {
		if (error?.response.status === 401) {
			const refreshToken = localStorage.getItem(
				REFRESH_TOKEN_LOCAL_STORAGE_KEY,
			);
			if (refreshToken) {
				const newAccessToken = await refreshAccessToken(refreshToken);
				const { data } = await axios.get(url, {
					headers: {
						Authorization: `Bearer ${newAccessToken}`,
					},
				});

				return data;
			}
		}

		throw error;
	}
};

export const axiosPost = async (url: string, request: any) => {
	try {
		const accessToken = localStorage.getItem(
			ACCESS_TOKEN_LOCAL_STORAGE_KEY,
		);

		const { data } = await axios.post(url, request, {
			headers: { Authorization: `Bearer ${accessToken}` },
		});

		return data;
	} catch (error: any) {
		if (error?.response.status === 401) {
			const refreshToken = localStorage.getItem(
				REFRESH_TOKEN_LOCAL_STORAGE_KEY,
			);
			if (refreshToken) {
				const newAccessToken = await refreshAccessToken(refreshToken);
				const { data } = await axios.get(url, {
					headers: {
						Authorization: `Bearer ${newAccessToken}`,
					},
				});

				return data;
			}
		}

		throw error;
	}
};

export const axiosPatch = async (url: string, request: any) => {
    try {
        const accessToken = localStorage.getItem(
            ACCESS_TOKEN_LOCAL_STORAGE_KEY,
        );

        const { data } = await axios.patch(url, request, {
            headers: { Authorization: `Bearer ${accessToken}` },
        });

        return data;
    } catch (error: any) {
        if (error?.response.status === 401) {
            const refreshToken = localStorage.getItem(
                REFRESH_TOKEN_LOCAL_STORAGE_KEY,
            );
            if (refreshToken) {
                const newAccessToken = await refreshAccessToken(refreshToken);
                const { data } = await axios.patch(url, {
                    headers: {
                        Authorization: `Bearer ${newAccessToken}`,
                    },
                });

                return data;
            }
        }

        throw error;
    }
};