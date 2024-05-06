import 'reflect-metadata';
import { inject, injectable } from 'inversify';
import { catchError, map, switchMap } from 'rxjs';
import { ajax } from 'rxjs/internal/ajax/ajax';
import { AuthDataStore } from '../../stores/auth.data-store.ts';
import { AjaxError } from 'rxjs/internal/ajax/errors';
import { ACCESS_TOKEN_LOCAL_STORAGE_KEY } from '../types/auth.type.ts';

@injectable()
export class HttpClientService {
	@inject(AuthDataStore)
	private readonly authDataStore!: AuthDataStore;

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
