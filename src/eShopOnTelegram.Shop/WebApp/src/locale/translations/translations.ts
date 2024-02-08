import en from './en.json';
import ru from './ru.json';

import { Translations } from '../types';
import { LanguageCode } from '../enums';

export const TRANSLATIONS: Translations = {
	[LanguageCode.EN]: en,
	[LanguageCode.RU]: ru,
};
