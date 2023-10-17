import React from 'react';
import { Datagrid, EditButton, List, TextField } from 'react-admin';

export default function ProductCategoriesList() {
	return (
		<List>
			<Datagrid bulkActionButtons={false}>
				<TextField source="name" sortable={true} />
				<EditButton />
			</Datagrid>
		</List>
	);
}
