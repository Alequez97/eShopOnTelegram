import {
	ReactNode,
	createContext,
	useContext,
	useEffect,
	useState,
} from 'react';
import { Translations } from '../types/translations.type';
import axios from 'axios';

const TranslationsContext = createContext<Translations | undefined>(undefined);

interface TranslationsProviderProps {
	children: ReactNode;
}

export const TranslationsProvider = ({
	children,
}: TranslationsProviderProps) => {
	const [translations, setTranslations] = useState<Translations>({
		allCategories: 'All categories',
		continue: 'Continue',
		noAvailableProducts: 'No available products at this moment',
		proceedToPayment: 'Proceed to payment',
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
		<TranslationsContext.Provider value={translations}>
			{children}
		</TranslationsContext.Provider>
	);
};

export const useTranslations = () => {
	const context = useContext(TranslationsContext);

	if (!context) {
		throw new Error(
			'useTranslations must be used within a TranslationsProvider',
		);
	}

	return context;
};
