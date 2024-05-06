import { ContainerModule } from 'inversify';
import { ProductsDataStore } from './stores/products.data-store.ts';
import { AuthDataStore } from './stores/auth.data-store.ts';
import { HttpClientService } from './_common/http/http-client.service.ts';

export class DIContainer extends ContainerModule {
	constructor() {
		super((bind) => {
			bind(HttpClientService).to(HttpClientService);
			bind(AuthDataStore).to(AuthDataStore);
			bind(ProductsDataStore).to(ProductsDataStore);
		});
	}
}
