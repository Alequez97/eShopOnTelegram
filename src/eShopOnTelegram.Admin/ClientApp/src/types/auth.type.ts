export interface LoginRequest {
	username: string;
	password: string;
}

export interface LoginResponse {
	accessToken: string;
	refreshToken: string;
}

export const ACCESS_TOKEN_LOCAL_STORAGE_KEY = 'eShopOnTelegram.AccessToken';
export const REFRESH_TOKEN_LOCAL_STORAGE_KEY = 'eShopOnTelegram.RefreshToken';
