import 'reflect-metadata';
import { injectable } from 'inversify';
import { makeAutoObservable, runInAction } from 'mobx';
import { LoginResponse } from '../_common/types/auth.type.ts';
import { ajax } from 'rxjs/internal/ajax/ajax';
import { AjaxResponse } from 'rxjs/internal/ajax/AjaxResponse';
import { map, tap } from 'rxjs';

const ACCESS_TOKEN_LOCAL_STORAGE_KEY = 'eShopOnTelegram.AccessToken';
const REFRESH_TOKEN_LOCAL_STORAGE_KEY = 'eShopOnTelegram.RefreshToken';

interface State {
	accessToken?: string;
}

@injectable()
export class AuthDataStore {
	private state: State = {
		accessToken: undefined,
	};

	constructor() {
		makeAutoObservable(this, undefined, { autoBind: true });

		const accessToken = localStorage.getItem(
			ACCESS_TOKEN_LOCAL_STORAGE_KEY,
		);
		if (accessToken) {
			runInAction(() => {
				this.state.accessToken = accessToken;
			});
		}
	}

	get accessToken() {
		return this.state.accessToken;
	}

	login$(username: string, password: string) {
		return ajax<LoginResponse>({
			method: 'POST',
			url: '/api/auth/login',
			body: {
				username,
				password,
			},
		}).pipe(tap((response) => this.handleLoginResponse(response)));
	}

	refresh$() {
		const refreshToken = localStorage.getItem(
			REFRESH_TOKEN_LOCAL_STORAGE_KEY,
		);

		if (!refreshToken) {
			throw new Error('Refresh token is missing');
		}

		return ajax<LoginResponse>({
			method: 'POST',
			url: '/api/auth/token/refresh',
			body: {
				refreshToken,
			},
		}).pipe(
			tap((response) => this.handleLoginResponse(response)),
			map(() => {
				const newAccessToken = localStorage.getItem(
					ACCESS_TOKEN_LOCAL_STORAGE_KEY,
				);

				if (!newAccessToken) {
					throw new Error('Failed to refresh access token');
				}

				return newAccessToken;
			}),
		);
	}

	logout() {
		runInAction(() => {
			this.state.accessToken = undefined;
			localStorage.removeItem(ACCESS_TOKEN_LOCAL_STORAGE_KEY);
			localStorage.removeItem(REFRESH_TOKEN_LOCAL_STORAGE_KEY);
		});
	}

	private handleLoginResponse = (response: AjaxResponse<LoginResponse>) => {
		const {
			response: { accessToken, refreshToken },
		} = response;

		runInAction(() => {
			this.state.accessToken = accessToken;
			localStorage.setItem(ACCESS_TOKEN_LOCAL_STORAGE_KEY, accessToken);
			localStorage.setItem(REFRESH_TOKEN_LOCAL_STORAGE_KEY, refreshToken);
		});
	};
}
