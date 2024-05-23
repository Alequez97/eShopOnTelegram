import { observer } from 'mobx-react-lite';
import { useInjection } from 'inversify-react';
import { ProductsDataStore } from '../stores/products.data-store.ts';
import {
	HStack,
	IconButton,
	List,
	Spinner,
	Table,
	TableContainer,
	Tbody,
	Td,
	Th,
	Thead,
	Tr,
} from '@chakra-ui/react';
import { IconPencil, IconTrash } from '@tabler/icons-react';
import { Pagination } from '../_common/components/pagination';

export const PageProducts = observer(() => {
	const {
		isLoading,
		hasError,
		pageNumber,
		totalPages,
		setPageNumber,
		products,
	} = useInjection(ProductsDataStore);

	if (isLoading) {
		return <Spinner />;
	}

	if (hasError) {
		return <div>Error...</div>;
	}

	return (
		<List>
			<TableContainer whiteSpace="pre-line">
				<Table variant="simple">
					<Thead>
						<Tr key={'table-header'}>
							<Th key={'table-header-id'}>Id</Th>
							<Th key={'table-header-name'}>Name</Th>
							<Th key={'table-header-product-category-name'}>
								Product category name
							</Th>
							<Th key={'table-header-actions'}>Actions</Th>
						</Tr>
					</Thead>
					<Tbody>
						{products.map((product) => (
							<Tr>
								<Td key={product.id}>{product.id}</Td>
								<Td key={product.name}>{product.name}</Td>
								<Td key={product.productCategoryName}>
									{product.productCategoryName}
								</Td>
								<Td>
									<HStack>
										<IconButton
											icon={<IconPencil />}
											aria-label={'Edit'}
											onClick={() => {
												console.log('EDIT');
											}}
										/>
										<IconButton
											icon={<IconTrash />}
											aria-label={'Edit'}
											onClick={() => {
												console.log('DELETE');
											}}
										/>
									</HStack>
								</Td>
							</Tr>
						))}
					</Tbody>
				</Table>
			</TableContainer>
			{totalPages && (
				<Pagination
					current={pageNumber}
					pageCount={totalPages}
					setCurrent={(pageNumber) => setPageNumber(pageNumber)}
				/>
			)}
		</List>
	);
});
