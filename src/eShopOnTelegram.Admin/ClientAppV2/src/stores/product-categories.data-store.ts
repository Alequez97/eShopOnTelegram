import { ProductCategory } from '../_common/types/api/product-category.type.ts';
import { inject, injectable } from 'inversify';
import { HttpClientService } from '../_common/http/http-client.service.ts';
import { Subscription, tap } from 'rxjs';
import {
	makeAutoObservable,
	onBecomeObserved,
	onBecomeUnobserved,
	runInAction,
} from 'mobx';
import { ApiResponse } from '../_common/types/api/api-response.type.ts';
import { Product } from '../_common/types/api/product.type.ts';
import { AjaxError } from 'rxjs/internal/ajax/errors';
import qs from 'qs';

interface State {
	isLoading: boolean;
	loadingError: Error | undefined;
	pageNumber: number;
	itemsPerPage: number;
	totalPages: number | undefined;
	productCategories: ProductCategory[];
}

@injectable()
export class ProductCategoriesDataStore {
	private state: State = {
		isLoading: false,
		loadingError: undefined,
		pageNumber: 1,
		itemsPerPage: 10,
		totalPages: undefined,
		productCategories: [],
	};

	@inject(HttpClientService)
	private readonly httpClient!: HttpClientService;

	private fetchSubscription?: Subscription;

	constructor() {
		makeAutoObservable(this, undefined, { autoBind: true });

		onBecomeObserved(
			this,
			'isLoading',
			() => (this.fetchSubscription = this.fetchProductCategories()),
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

	get productCategories() {
		return this.state.productCategories;
	}

	get pageNumber() {
		return this.state.pageNumber;
	}

	get totalPages() {
		return this.state.totalPages;
	}

	setPageNumber(pageNumber: number) {
		this.state.pageNumber = pageNumber;
	}

	get itemsPerPage() {
		return this.state.itemsPerPage;
	}

	private fetchProductCategories() {
		const queryParams = qs.stringify({
			pageNumber: this.state.pageNumber,
			itemsPerPage: this.state.itemsPerPage,
		});

		return this.httpClient
			.get$<ApiResponse<Product[]>>(
				`/api/productCategories?${queryParams}`,
			)
			.pipe(
				tap(() =>
					runInAction(() => {
						this.state.isLoading = true;
					}),
				),
			)
			.subscribe({
				next: (response) => {
					runInAction(() => {
						this.state.isLoading = false;
						this.state.productCategories = response.data;
						this.state.totalPages = response.metadata.totalPages;
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
