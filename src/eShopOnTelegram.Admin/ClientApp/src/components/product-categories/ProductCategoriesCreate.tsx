import React from 'react';
import { required, SimpleForm, TextInput, useNotify } from 'react-admin';
import { axiosPost } from '../../utils/axios.utility';
import { FieldValues } from 'react-hook-form';

export default function ProductCategoriesCreate() {
	const notify = useNotify();

	const onSubmitHandler = async (request: FieldValues) => {
		try {
			await axiosPost('/productCategories', request);
			notify('New category created', { type: 'success' });
		} catch (error: unknown) {
			notify('Unable to create new category', { type: 'error' });
		}
	};

	return (
		<SimpleForm onSubmit={onSubmitHandler}>
			<TextInput source="name" validate={required()} />
		</SimpleForm>
	);
}
