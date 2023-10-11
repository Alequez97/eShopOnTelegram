// AuthProvider.ts
import { ACCESS_TOKEN_LOCAL_STORAGE_KEY, LoginRequest, LoginResponse, REFRESH_TOKEN_LOCAL_STORAGE_KEY } from './types/auth.type';
import axios, { AxiosError } from 'axios';

export const authProvider = {
    login: async ({ username, password }: LoginRequest) => {
        try {
            const response = await axios.post('/auth/login', {
                username,
                password,
            });
    
            const auth = response.data as LoginResponse;
            localStorage.setItem(ACCESS_TOKEN_LOCAL_STORAGE_KEY, auth.accessToken);
            localStorage.setItem(REFRESH_TOKEN_LOCAL_STORAGE_KEY, auth.refreshToken);
        } catch (error: any) {
            if (error?.response?.status === 400) {
                throw new Error('Wrong credentials')
            }

            throw new Error('Network error');
        }
    },
    logout: async () => {
        localStorage.removeItem(ACCESS_TOKEN_LOCAL_STORAGE_KEY);
        localStorage.removeItem(REFRESH_TOKEN_LOCAL_STORAGE_KEY);
        await Promise.resolve();
    },
    checkAuth: ({ username, password }: LoginRequest) => {
        return new Promise<void>((resolve, reject) => {
            const accessToken = localStorage.getItem(ACCESS_TOKEN_LOCAL_STORAGE_KEY);

            if (!accessToken) {
                reject();
            }

            resolve();
        });
    },
    // when the dataProvider returns an error, check if this is an authentication error
    checkError: (error: any) => Promise.resolve(),
    // get the user's profile
    getIdentity: () => Promise.resolve(),
    // get the user permissions (optional)
    getPermissions: () => Promise.resolve(),
};