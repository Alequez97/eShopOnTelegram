import React from 'react';
import { Datagrid, EditButton, List, SimpleList, TextField } from 'react-admin';
import { useMediaQuery } from 'react-responsive';
import { Order, ProductCategory } from '../../types/api-response.type';

export default function ProductCategoriesList() {
	const isMobile = useMediaQuery({ query: `(max-width: 760px)` });

	return isMobile ? (
		<List>
			<SimpleList
				primaryText={(category: ProductCategory) => `${category.name}`}
				rowSx={() => ({ border: '1px solid #eee' })}
				linkType={'edit'}
			/>
		</List>
	) : (
		<List>
			<Datagrid bulkActionButtons={false}>
				<TextField source="name" sortable={true} />
				<EditButton />
			</Datagrid>
		</List>
	);
}
