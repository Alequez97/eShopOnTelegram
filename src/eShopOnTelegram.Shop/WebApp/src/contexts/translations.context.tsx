import {
	createContext,
	ReactNode,
	useContext,
	useEffect,
	useState,
} from 'react';
import { BackendTranslations } from '../types/translations.type';
import axios from 'axios';
import { LanguageCode } from '../locale';

const BackendTranslationsContext = createContext<
	BackendTranslations | undefined
>(undefined);

interface BackendTranslationsProviderProps {
	children: ReactNode;
}

export const BackendTranslationsProvider = ({
	children,
}: BackendTranslationsProviderProps) => {
	const [translations, setTranslations] = useState<BackendTranslations>({
		allCategories: 'All categories',
		continue: 'Continue',
		noAvailableProducts: 'No available products at this moment',
		proceedToPayment: 'Proceed to payment',
		totalPrice: 'Total price',
		currencySymbol: '€',
		language: LanguageCode.EN,
	});

	useEffect(() => {
		const fetchTranslations = async () => {
			try {
				const response = await axios.get('/translations');
				setTranslations(response.data);
			} catch (error) {
				console.error('Error fetching translations:', error);
			}
		};

		fetchTranslations();
	}, []);

	return (
		<BackendTranslationsContext.Provider value={translations}>
			{children}
		</BackendTranslationsContext.Provider>
	);
};

export const useBackendTranslations = () => {
	const context = useContext(BackendTranslationsContext);

	if (!context) {
		throw new Error(
			'useTranslations must be used within a TranslationsProvider',
		);
	}

	return context;
};
