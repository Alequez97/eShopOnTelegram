import React, { useEffect, useState } from 'react';
import { axiosGet, axiosPost } from '../../utils/axios.utility';
import {
	PasswordInput,
	required,
	SelectInput,
	SimpleForm,
	useAuthenticated,
	useNotify,
} from 'react-admin';
import { FieldValues } from 'react-hook-form';

interface SecretsConfigItem {
	displayName: string;
	publicSecretName: string;
}

export const SecretsCreate = () => {
	useAuthenticated();

	const notify = useNotify();
	const [secretsConfigItems, setSecretsConfigItems] = useState<
		SecretsConfigItem[]
	>([]);
	const [isLoading, setIsLoading] = useState(false);

	useEffect(() => {
		const fetchConfig = async () => {
			try {
				setIsLoading(true);
				const secretsConfig = await axiosGet('/secretsConfig');
				setSecretsConfigItems(secretsConfig);
			} catch {
				notify('Error. Try again later', { type: 'error' });
			} finally {
				setIsLoading(false);
			}
		};

		fetchConfig();
	}, []);

	if (isLoading) {
		return <div>Loading...</div>;
	}

	const handleSecretUpdate = async (request: FieldValues) => {
		try {
			setIsLoading(true);
			console.log(request);
			await axiosPost('/secretsConfig', request);
			notify('Secret successfully updated', { type: 'success' });
		} catch {
			notify('Was not able to update secret. Try again later', {
				type: 'error',
			});
		} finally {
			setIsLoading(false);
		}
	};

	return (
		<SimpleForm onSubmit={handleSecretUpdate}>
			<SelectInput
				choices={secretsConfigItems}
				source={'publicSecretName'}
				label={'Secret name'}
				optionText={'displayName'}
				optionValue={'publicSecretName'}
				validate={[required()]}
			/>
			<PasswordInput
				label="Token"
				source="value"
				validate={[required()]}
			/>
		</SimpleForm>
	);
};
