import React from 'react';
import {
	Create,
	PasswordInput,
	required,
	SimpleForm,
	TextInput,
	useNotify,
} from 'react-admin';
import { axiosPost } from '../../utils/axios.utility';

const validatePassword = (value: any) => {
	if (value.length < 6) {
		return 'Passwords must be at least 6 characters.';
	}

	if (!/[a-z]/.test(value)) {
		return "Passwords must have at least one lowercase ('a'-'z').";
	}

	if (!/[A-Z]/.test(value)) {
		return "Passwords must have at least one uppercase ('A'-'Z').";
	}

	return undefined;
};

export const UserCreate = () => {
	const notify = useNotify();

	const onSubmitHandler = async (request: any) => {
		try {
			await axiosPost('/users', request);
			notify('New user created', { type: 'success' });
		} catch (error: any) {
			notify('Unable to create new user', { type: 'error' });
		}
	};

	return (
		<Create title="Create new user">
			<SimpleForm onSubmit={onSubmitHandler}>
				<TextInput source="username" validate={required()} />
				<PasswordInput
					source="password"
					validate={[required(), validatePassword]}
				/>
			</SimpleForm>
		</Create>
	);
};
