import React from 'react';
import {
	Create,
	PasswordInput,
	required,
	SimpleForm,
	TextInput,
	useNotify,
} from 'react-admin';
import {
	ACCESS_TOKEN_LOCAL_STORAGE_KEY,
	REFRESH_TOKEN_LOCAL_STORAGE_KEY,
} from '../../types/auth.type';
import axios from 'axios';
import { refreshAccessToken } from '../../utils/auth.utility';

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
			const accessToken = localStorage.getItem(
				ACCESS_TOKEN_LOCAL_STORAGE_KEY,
			);
			await axios.post('/users', request, {
				headers: { Authorization: `Bearer ${accessToken}` },
			});
			notify('New user created', { type: 'success' });
		} catch (error: any) {
			if (error?.response.status === 401) {
				const refreshToken = localStorage.getItem(
					REFRESH_TOKEN_LOCAL_STORAGE_KEY,
				);

				if (refreshToken) {
					try {
						const newAccessToken =
							await refreshAccessToken(refreshToken);
						await axios.patch('/applicationContent', request, {
							headers: {
								Authorization: `Bearer ${newAccessToken}`,
							},
						});
						notify('New user created', { type: 'success' });
					} catch {
						notify('Unable to create new user', {
							type: 'error',
						});
					}
				} else {
					notify('Unable to create new user', { type: 'error' });
				}
			} else {
				notify('Unable to create new user', { type: 'error' });
			}
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
