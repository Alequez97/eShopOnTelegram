import i18next from 'i18next';
import {
	DEFAULT_LANGUAGE_CODE,
	LanguageCode,
	TranslationsNamespace,
} from '../enums';
import { TRANSLATIONS } from '../translations/translations';

const i18nInstance = i18next.createInstance();

void i18nInstance.init({
	resources: {
		[DEFAULT_LANGUAGE_CODE]: TRANSLATIONS[DEFAULT_LANGUAGE_CODE],
	},
	lng: DEFAULT_LANGUAGE_CODE,
	fallbackLng: DEFAULT_LANGUAGE_CODE,
	defaultNS: TranslationsNamespace.COMMON,
	fallbackNS: TranslationsNamespace.COMMON,
	ns: [TranslationsNamespace.COMMON],
});

for (const languageCode of Object.values(LanguageCode)) {
	i18nInstance.addResources(
		languageCode,
		TranslationsNamespace.COMMON,
		TRANSLATIONS[languageCode],
	);
}

export default i18nInstance;
