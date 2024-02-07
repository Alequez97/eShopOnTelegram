import { useEffect } from 'react';
import { getCartItemsAsJsonString } from '../../utils/cart-items.utility';
import { useCartItemsStore } from '../../contexts/cart-items-store.context';
import { useTelegramWebApp } from './useTelegramWebApp';

export const useSendCartItemToTelegram = () => {
	const telegramWebApp = useTelegramWebApp();
	const cartItemsStore = useCartItemsStore();

	useEffect(() => {
		const notEmptyCartItems = cartItemsStore.cartItemsState.filter(
			(cartItem) => cartItem.quantity > 0,
		);

		const sendDataToTelegram = () => {
			const json = getCartItemsAsJsonString(notEmptyCartItems);
			telegramWebApp.sendData(json);
		};

		if (notEmptyCartItems.length === 0) {
			telegramWebApp.MainButton.hide();
		} else {
			telegramWebApp.MainButton.onClick(sendDataToTelegram);
			telegramWebApp.MainButton.show();
		}

		return () => {
			telegramWebApp.MainButton.offClick(sendDataToTelegram);
		};
	}, [cartItemsStore.cartItemsState]);
};
