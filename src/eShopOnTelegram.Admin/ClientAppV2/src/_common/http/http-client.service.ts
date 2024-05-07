import 'reflect-metadata';
import { inject, injectable } from 'inversify';
import { catchError, EMPTY, map, switchMap } from 'rxjs';
import { ajax } from 'rxjs/internal/ajax/ajax';
import { AuthDataStore } from '../../stores/auth.data-store.ts';
import { AjaxError } from 'rxjs/internal/ajax/errors';
import { RouterLocationStore } from '../router/router-location.store.ts';
import { RouterLocation } from '../router/router-location.type.ts';
import { toJS } from 'mobx';

@injectable()
export class HttpClientService {
	@inject(AuthDataStore)
	private readonly authDataStore!: AuthDataStore;

	@inject(RouterLocationStore)
	private readonly routerLocation!: RouterLocation;

	get$<TResponse>(path: string) {
		return ajax<TResponse>(
			this.getRequestConfig('GET', path, this.authDataStore.accessToken!),
		).pipe(
			catchError((error: AjaxError) => {
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
			catchError((error: AjaxError) => {
				if (error.status === 400) {
					console.log(toJS(this.routerLocation.search));
					this.routerLocation.navigate('/login');
					return EMPTY;
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
