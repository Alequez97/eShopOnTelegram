import { CartItem } from '../types/cart-item.type';
import { FieldValues } from 'react-hook-form';

export function getOrderCreationRequestBodyAsJsonString(
	cartItems: CartItem[],
	deliveryInformation: FieldValues,
) {
	const products: string[] = [];

	cartItems.forEach((cartItem) => {
		products.push(
			`{"productAttributeId":"${cartItem.productAttribute.id}", "originalPrice":${cartItem.productAttribute.originalPrice}, "priceWithDiscount":${cartItem.productAttribute?.priceWithDiscount}, "quantity":${cartItem.quantity}}`,
		);
	});

	return `{
		"cartItems":[${products.join(',')}],
		"country": "${deliveryInformation.country}",
		"city": "${deliveryInformation.city}",
		"streetLine1": "${deliveryInformation.streetLine1}",
		"streetLine2": ${
			deliveryInformation.streetLine2
				? `"${deliveryInformation.streetLine2}"`
				: null
		},
		"postCode": "${deliveryInformation.postCode}",
	}`;
}
