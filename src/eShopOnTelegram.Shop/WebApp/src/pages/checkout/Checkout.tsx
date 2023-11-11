import { useCartItemsStore } from '../../contexts/cart-items-store.context';

export const Checkout = () => {
	const cartItemsStore = useCartItemsStore();

	return <div>{JSON.stringify(cartItemsStore.cartItemsState)}</div>;
};
