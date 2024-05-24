import { observer } from 'mobx-react-lite';
import { useInjection } from 'inversify-react';
import { ProductCategoriesDataStore } from '../stores/product-categories.data-store.ts';
import {
	HStack,
	IconButton,
	List,
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
import { Loader } from '../_common/components/loader';

export const PageProductCategories = observer(() => {
	const {
		isLoading,
		hasError,
		totalPages,
		pageNumber,
		setPageNumber,
		productCategories,
	} = useInjection(ProductCategoriesDataStore);

	if (isLoading) {
		return <Loader />;
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
							<Th key={'table-header-name'}>Category name</Th>
							<Th key={'table-header-actions'}>Actions</Th>
						</Tr>
					</Thead>
					<Tbody>
						{productCategories.map((productCategory) => (
							<Tr>
								<Td key={productCategory.id}>
									{productCategory.id}
								</Td>
								<Td key={productCategory.name}>
									{productCategory.name}
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
