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
	debounceTime,
	delay,
	Subscription,
	switchMap,
	tap,
} from 'rxjs';
import { Product } from '../_common/types/api/product.type.ts';
import { HttpClientService } from '../_common/http/http-client.service.ts';
import { AjaxError } from 'rxjs/internal/ajax/errors';
import { toObservable } from '../_common/utilities/observable.utility.ts';
import qs from 'qs';

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

	setPageNumber(pageNumber: number) {
		this.state.pageNumber = pageNumber;
	}

	get itemsPerPage() {
		return this.state.itemsPerPage;
	}

	setItemsPerPage(itemsPerPage: number) {
		this.state.itemsPerPage = itemsPerPage;
	}

	private fetchProducts() {
		return combineLatest([
			toObservable(() => this.state.pageNumber),
			toObservable(() => this.state.itemsPerPage),
		])
			.pipe(
				debounceTime(500),
				tap(() =>
					runInAction(() => {
						this.state.isLoading = true;
					}),
				),
				switchMap(([pageNumber, itemsPerPage]) => {
					const queryParams = qs.stringify({
						pageNumber,
						itemsPerPage,
					});

					return this.httpClient
						.get$<Product[]>(`/api/products?${queryParams}`)
						.pipe(
							tap(() =>
								runInAction(() => {
									this.state.isLoading = true;
								}),
							),
							delay(1000),
						);
				}),
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
