import { observer } from 'mobx-react-lite';
import { useInjection } from 'inversify-react';
import { AuthDataStore } from '../stores/auth.data-store.ts';
import { ProductsDataStore } from '../stores/products.data-store.ts';

export const PageProducts = observer(() => {
	const { isAuthenticated, login$ } = useInjection(AuthDataStore);
	const { isLoading, hasError, products } = useInjection(ProductsDataStore);

	if (!isAuthenticated) {
		// redirect to login page
		return (
			<div>
				<h3>NOT AUTH</h3>
				<button onClick={() => login$('admin', '1234').subscribe()}>
					Login
				</button>
			</div>
		);
	}

	if (isLoading) {
		<div>Loading...</div>;
	}

	if (hasError) {
		<div>Error...</div>;
	}

	return (
		<div>
			<h1>{JSON.stringify(products)}</h1>
		</div>
	);
});
