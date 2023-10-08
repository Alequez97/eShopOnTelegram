import { createContext, MutableRefObject } from 'react';

export const DEFAULT_TELEGRAM_WEBAPP =
  typeof window !== 'undefined' && window?.Telegram?.WebApp
    ? window.Telegram.WebApp
    : null;
DEFAULT_TELEGRAM_WEBAPP.MainButton.hide();

export const telegramWebAppContext =
  createContext<typeof DEFAULT_TELEGRAM_WEBAPP>(DEFAULT_TELEGRAM_WEBAPP);

/**
 * This object describe options be able to set through WebAppProvider
 */
export type Options = {
  /**
   * When is `true`, we can smooth button transitions due to show(), hide() calls.
   * So when you use MainButton or BackButton on multiple pages, there will be
   * no noticeable flickering of the button during transitions
   * @defaultValue `false`
   */
  smoothButtonsTransition?: boolean;
  /**
   * @defaultValue `10`
   * @remarks
   */
  smoothButtonsTransitionMs?: number;
};

export const DEFAULT_TELEGRAM_OPTIONS: Options = {
  smoothButtonsTransition: false,
  smoothButtonsTransitionMs: 100,
};

export const telegramOptionsContext = createContext<Options>(DEFAULT_TELEGRAM_OPTIONS);

type TelegramContext = {
  MainButton: MutableRefObject<null | string>;
  BackButton: MutableRefObject<null | string>;
};

export const createTelegramContextValue = () => ({
  MainButton: { current: null },
  BackButton: { current: null },
});

export const telegramContext = createContext<TelegramContext>(
  createTelegramContextValue(),
);