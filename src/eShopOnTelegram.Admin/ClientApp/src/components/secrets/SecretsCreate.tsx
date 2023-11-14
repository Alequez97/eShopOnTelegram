import React, { useEffect, useState } from 'react';
import { axiosGet } from '../../utils/axios.utility';
import {
	PasswordInput,
	required,
	SelectInput,
	SimpleForm,
	useAuthenticated,
	useNotify,
} from 'react-admin';

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

	return (
		<SimpleForm onSubmit={(request) => console.log(request)}>
			<SelectInput
				choices={secretsConfigItems}
				source={'publicSecretName'}
				label={'Secret name'}
				optionText={'displayName'}
				optionValue={'publicSecretName'}
				validate={[required()]}
			/>
			<PasswordInput source="token" validate={[required()]} />
		</SimpleForm>
	);
};
