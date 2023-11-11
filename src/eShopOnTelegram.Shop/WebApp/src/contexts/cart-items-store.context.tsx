import React, { createContext, ReactNode, useContext } from 'react';
import { CartItemsStore } from '../stores/cart-items.store';

const CartItemsStoreContext = createContext<CartItemsStore | undefined>(
	undefined,
);

interface CartItemsStoreProviderProps {
	children: ReactNode;
}

export const CartItemsStoreProvider = ({
	children,
}: CartItemsStoreProviderProps) => {
	const cartItemsStore = new CartItemsStore();

	return (
		<CartItemsStoreContext.Provider value={cartItemsStore}>
			{children}
		</CartItemsStoreContext.Provider>
	);
};

export const useCartItemsStore = () => {
	const context = useContext(CartItemsStoreContext);
	if (!context) {
		throw new Error(
			'useCartItemsStore must be used within a CartItemsStoreProvider',
		);
	}
	return context;
};
