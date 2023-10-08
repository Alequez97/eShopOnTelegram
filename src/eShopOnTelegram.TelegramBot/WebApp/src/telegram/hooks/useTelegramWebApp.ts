import { useContext } from 'react';
import { telegramWebAppContext } from '../telegram-context';

export const useTelegramWebApp = () => {
  const context = useContext(telegramWebAppContext);

  return context;
};