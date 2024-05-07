import 'reflect-metadata';
import {
	makeAutoObservable,
	onBecomeObserved,
	onBecomeUnobserved,
	runInAction,
} from 'mobx';
import { LoadingState } from '../_common/types/store-state.type.ts';
import { inject, injectable } from 'inversify';
import {
	combineLatest,
	debounce,
	delay,
	of,
	Subscription,
	switchMap,
	tap,
} from 'rxjs';
import { Product } from '../_common/types/api/product.type.ts';
import { HttpClientService } from '../_common/http/http-client.service.ts';
import { AjaxError } from 'rxjs/internal/ajax/errors';
import { toObservable } from '../_common/utilities/observable.utility.ts';

interface State {
	isLoading: boolean;
	loadingError: Error | undefined;
	pageNumber: number;
	itemsPerPage: number;
	products: Product[];
}

@injectable()
export class ProductsDataStore implements LoadingState {
	private state: State = {
		isLoading: false,
		loadingError: undefined,
		pageNumber: 1,
		itemsPerPage: 5,
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

	setPageNumber(number: number) {
		this.state.pageNumber += number;
	}

	private fetchProducts() {
		// combineLatest([toObservable(() => this.state.pageNumber)])
		// 	.pipe(
		// 		switchMap((paginationAndFilteringData) =>
		// 			this.httpClient.get$<Product[]>('/api/products').pipe(
		// 				tap(() =>
		// 					runInAction(() => {
		// 						this.state.isLoading = true;
		// 					}),
		// 				),
		// 				delay(1000),
		// 			),
		// 		),
		// 	)
		// 	.subscribe(); // TODO: Uncomment and replace with this code, when pagination is ready

		return this.httpClient
			.get$<Product[]>('/api/products')
			.pipe(
				tap(() =>
					runInAction(() => {
						this.state.isLoading = true;
					}),
				),
				delay(1000),
			)
			.subscribe({
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
