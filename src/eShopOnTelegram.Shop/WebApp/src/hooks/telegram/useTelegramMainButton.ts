import { useTelegramWebApp } from './useTelegramWebApp';
import { useEffect } from 'react';

export const useTelegramMainButton = (
	isVisible: boolean,
	callback: () => void,
	buttonText?: string,
) => {
	const telegramWebApp = useTelegramWebApp();

	if (buttonText) {
		telegramWebApp.MainButton.setText(buttonText);
	}

	useEffect(() => {
		if (isVisible) {
			telegramWebApp.MainButton.show();
		} else {
			telegramWebApp.MainButton.hide();
		}

		telegramWebApp.MainButton.onClick(callback);

		return () => {
			telegramWebApp.MainButton.offClick(callback);
		};
	}, [isVisible, callback]);
};
