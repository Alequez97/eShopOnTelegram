import { WebApp } from '../../types/telegram';

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
