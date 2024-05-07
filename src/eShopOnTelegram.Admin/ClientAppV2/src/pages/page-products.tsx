import { observer } from 'mobx-react-lite';
import { useInjection } from 'inversify-react';
import { AuthDataStore } from '../stores/auth.data-store.ts';
import { ProductsDataStore } from '../stores/products.data-store.ts';
import { Spinner } from '@chakra-ui/react';

export const PageProducts = observer(() => {
	const { isAuthenticated } = useInjection(AuthDataStore);
	const { isLoading, hasError, setPageNumber, products } =
		useInjection(ProductsDataStore);

	if (!isAuthenticated) {
		// redirect to login page
		return (
			<div>
				<h3>NOT AUTH</h3>
			</div>
		);
	}

	if (isLoading) {
		return <Spinner />;
	}

	if (hasError) {
		return <div>Error...</div>;
	}

	return (
		<div>
			<button onClick={() => setPageNumber(2)}>set page number</button>
			<h1>{JSON.stringify(products)}</h1>
		</div>
	);
});
