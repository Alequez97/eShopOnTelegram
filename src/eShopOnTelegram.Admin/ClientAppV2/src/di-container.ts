import { ContainerModule } from 'inversify';
import { ProductsDataStore } from './stores/products.data-store.ts';
import { AuthDataStore } from './stores/auth.data-store.ts';
import { HttpClientService } from './_common/http/http-client.service.ts';
import { RouterLocationStore } from './_common/router/router-location.store.ts';
import { RouterLocation } from './_common/router/router-location.type.ts';

export class DIContainer extends ContainerModule {
	constructor() {
		super((bind) => {
			bind(HttpClientService).to(HttpClientService).inSingletonScope();
			bind(AuthDataStore).to(AuthDataStore).inSingletonScope();
			bind(ProductsDataStore).to(ProductsDataStore).inSingletonScope();

			bind<RouterLocation>(RouterLocationStore)
				.to(RouterLocationStore)
				.inSingletonScope();
		});
	}
}
