import React from 'react';
import {
	PasswordInput,
	required,
	SimpleForm,
	TextInput,
	useAuthenticated,
	useNotify,
} from 'react-admin';
import { axiosPost } from '../../utils/axios.utility';
import { FieldValues } from 'react-hook-form';

const validatePassword = (value: string) => {
	if (value.length < 6) {
		return 'Passwords must be at least 6 characters.';
	}

	if (!/[a-z]/.test(value)) {
		return "Passwords must have at least one lowercase ('a'-'z').";
	}

	if (!/[A-Z]/.test(value)) {
		return "Passwords must have at least one uppercase ('A'-'Z').";
	}

	if (!/\W/.test(value)) {
		return 'Passwords must have at least one special character.';
	}

	return undefined;
};

export const UserCreate = () => {
	useAuthenticated();

	const notify = useNotify();

	const onSubmitHandler = async (request: FieldValues) => {
		try {
			await axiosPost('/users', request);
			notify('New user created', { type: 'success' });
		} catch (error: unknown) {
			notify('Unable to create new user', { type: 'error' });
		}
	};

	return (
		<SimpleForm onSubmit={onSubmitHandler}>
			<TextInput source="username" validate={required()} />
			<PasswordInput
				source="password"
				validate={[required(), validatePassword]}
			/>
		</SimpleForm>
	);
};
