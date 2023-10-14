import {useContext} from 'react';
import {telegramWebAppContext} from '../telegram-context';

export const useTelegramWebApp = () => {
    return useContext(telegramWebAppContext);
};
