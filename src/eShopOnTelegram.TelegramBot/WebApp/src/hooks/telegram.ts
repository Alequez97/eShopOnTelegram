export function useTelegramWebApp() {
    const telegramWebApp = window.Telegram.WebApp
    telegramWebApp.ready();

    return {
        telegramWebApp
    }
}