import { ReactNode, useEffect } from 'react';
import { I18nextProvider } from 'react-i18next';
import i18nInstance from './i18n-instance';
import { useBackendTranslations } from '../../contexts/translations.context';

export interface ClientSideTranslationsProviderProps {
	children: ReactNode;
}

export const ClientSideTranslationsProvider = ({
	children,
}: ClientSideTranslationsProviderProps) => {
	const backendTranslations = useBackendTranslations();

	useEffect(() => {
		void i18nInstance.changeLanguage(backendTranslations.language);
	}, [backendTranslations.language]);

	console.log(backendTranslations.language);

	return <I18nextProvider i18n={i18nInstance}>{children}</I18nextProvider>;
};
