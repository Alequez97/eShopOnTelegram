import { useEffect } from 'react';
import { useTelegramWebApp } from './useTelegramWebApp';

export const useTelegramBackButton = (callback: () => void) => {
	const telegramWebApp = useTelegramWebApp();

	useEffect(() => {
		telegramWebApp.BackButton?.show();

		telegramWebApp.BackButton?.onClick(callback);

		return () => {
			telegramWebApp.BackButton?.hide();
			telegramWebApp.BackButton?.offClick(callback);
		};
	}, []);
};
