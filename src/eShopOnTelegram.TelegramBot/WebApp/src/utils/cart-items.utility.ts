import { CartItem } from '../types/cart-item.type';

export function getCartItemsAsJsonString(cartItems: CartItem[]) {
	const products: string[] = [];

	cartItems.forEach((cartItem) => {
		products.push(
			`{"productAttributeId":"${cartItem.productAttribute.id}", "originalPrice":${cartItem.productAttribute.originalPrice}, "priceWithDiscount":${cartItem.productAttribute?.priceWithDiscount}, "quantity":${cartItem.quantity}}`,
		);
	});

	return `{"cartItems":[${products.join(',')}]}`;
}
