import { WebApp } from '../telegram/types';

let isReadyCalled = false;

export function useTelegramWebApp(): WebApp {
	const telegramWebApp = window.Telegram.WebApp;

	if (!isReadyCalled) {
		telegramWebApp.ready();
		telegramWebApp.expand();
		isReadyCalled = true;
	}

	return telegramWebApp;
}
