let isReadyCalled = false;

export function useTelegramWebApp() {
  const telegramWebApp = window.Telegram.WebApp;

  if (!isReadyCalled) {
    telegramWebApp.ready();
    isReadyCalled = true;
  }

  return {
    telegramWebApp,
  };
}
