import React from 'react';
import { required, SimpleForm, TextInput, useNotify } from 'react-admin';
import { axiosPost } from '../../utils/axios.utility';

export default function ProductCategoriesCreate() {
	const notify = useNotify();

	const onSubmitHandler = async (request: any) => {
		try {
			await axiosPost('/productCategories', request);
			notify('New category created', { type: 'success' });
		} catch (error: any) {
			notify('Unable to create new category', { type: 'error' });
		}
	};

	return (
		<SimpleForm onSubmit={onSubmitHandler}>
			<TextInput source="name" validate={required()} />
		</SimpleForm>
	);
}
