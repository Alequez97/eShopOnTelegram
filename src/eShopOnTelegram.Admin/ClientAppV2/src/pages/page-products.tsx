import { observer } from 'mobx-react-lite';
import { useInjection } from 'inversify-react';
import { ProductsDataStore } from '../stores/products.data-store.ts';
import { Spinner } from '@chakra-ui/react';

export const PageProducts = observer(() => {
	const { isLoading, hasError, itemsPerPage, setItemsPerPage, products } =
		useInjection(ProductsDataStore);

	if (isLoading) {
		return <Spinner />;
	}

	if (hasError) {
		return <div>Error...</div>;
	}

	return (
		<div>
			<input
				type={'text'}
				onChange={(event) =>
					setItemsPerPage(event.target.value as unknown as number)
				}
				value={itemsPerPage}
			/>
			<h1>{JSON.stringify(products)}</h1>
		</div>
	);
});
