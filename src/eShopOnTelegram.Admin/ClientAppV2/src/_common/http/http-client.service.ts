import 'reflect-metadata';
import { inject, injectable } from 'inversify';
import { catchError, map, switchMap, tap } from 'rxjs';
import { ajax } from 'rxjs/internal/ajax/ajax';
import { AuthDataStore } from '../../stores/auth.data-store.ts';
import { AjaxError } from 'rxjs/internal/ajax/errors';
import { ACCESS_TOKEN_LOCAL_STORAGE_KEY } from '../types/auth.type.ts';
import { RouterLocationStore } from '../router/router-location.store.ts';
import { RouterLocation } from '../router/router-location.type.ts';

@injectable()
export class HttpClientService {
	@inject(AuthDataStore)
	private readonly authDataStore!: AuthDataStore;

	@inject(RouterLocationStore)
	private readonly routerLocation!: RouterLocation;

	get$<TResponse>(path: string) {
		return ajax<TResponse>(
			this.getRequestConfig(
				'GET',
				path,
				localStorage.getItem(ACCESS_TOKEN_LOCAL_STORAGE_KEY)!,
			),
		).pipe(
			catchError((error: AjaxError, _) => {
				if (error.status === 401) {
					return this.authDataStore
						.refresh$()
						.pipe(
							switchMap((newAccessToken) =>
								ajax<TResponse>(
									this.getRequestConfig(
										'GET',
										path,
										newAccessToken,
									),
								),
							),
						);
				} else {
					throw error;
				}
			}),
			catchError((error: AjaxError, _) => {
				if (error.status === 400) {
					this.routerLocation.navigate('/login');
				} else {
					throw error;
				}
			}),
			map((x) => x.response),
		);
	}

	private getRequestConfig(
		method: string,
		path: string,
		accessToken: string,
	) {
		const headers = {
			Accept: 'application/json',
			'Content-Type': 'application/json',
		};

		return {
			method: method,
			url: path,
			headers: {
				...headers,
				Authorization: `Bearer ${accessToken}`,
			},
		};
	}
}
