import 'reflect-metadata';
import {
	makeAutoObservable,
	onBecomeObserved,
	onBecomeUnobserved,
	runInAction,
} from 'mobx';
import { LoadingState } from '../_common/types/store-state.type.ts';
import { inject, injectable } from 'inversify';
import { Subscription } from 'rxjs';
import { Product } from '../_common/types/api/product.type.ts';
import { HttpClientService } from '../_common/http/http-client.service.ts';
import { AjaxError } from 'rxjs/internal/ajax/errors';

interface State {
	isLoading: boolean;
	loadingError: Error | undefined;
	products: Product[];
}

@injectable()
export class ProductsDataStore implements LoadingState {
	private state: State = {
		isLoading: false,
		loadingError: undefined,
		products: [],
	};

	@inject(HttpClientService)
	private readonly httpClient!: HttpClientService;

	private fetchSubscription?: Subscription;

	constructor() {
		makeAutoObservable(this, undefined, { autoBind: true });

		onBecomeObserved(
			this,
			'isLoading',
			() => (this.fetchSubscription = this.fetchProducts()),
		);

		onBecomeUnobserved(this, 'isLoading', () =>
			this.fetchSubscription?.unsubscribe(),
		);
	}

	get isLoading() {
		return this.state.isLoading;
	}

	get hasError() {
		return !!this.state.loadingError;
	}

	get products() {
		return this.state.products;
	}

	private fetchProducts() {
		runInAction(() => {
			this.state.isLoading = true;
		});

		return this.httpClient.get$<Product[]>('/api/products').subscribe({
			next: (response) => {
				runInAction(() => {
					this.state.isLoading = false;
					this.state.products = response;
				});
			},
			error: (error: Error | AjaxError) => {
				runInAction(() => {
					this.state.isLoading = false;
					this.state.loadingError = error;
				});
			},
		});
	}
}
