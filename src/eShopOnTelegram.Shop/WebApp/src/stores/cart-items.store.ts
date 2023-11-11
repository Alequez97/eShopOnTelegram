import { makeAutoObservable } from 'mobx';
import { ProductAttribute } from '../types/product-attribute.type';
import { CartItem } from '../types/cart-item.type';

export class CartItemsStore {
	private cartItems: CartItem[] = [];

	constructor() {
		makeAutoObservable(this);
	}

	get cartItemsState() {
		return this.cartItems;
	}

	public addProductAttribute(productAttribute: ProductAttribute) {
		const existingCartItem = this.cartItems.find(
			(cartItem) => cartItem.productAttribute.id === productAttribute.id,
		);

		if (existingCartItem) {
			this.cartItems = this.cartItems.map((cartItem) => {
				if (cartItem.productAttribute.id === productAttribute.id) {
					return {
						...existingCartItem,
						quantity: existingCartItem.quantity + 1,
					};
				}
				return cartItem;
			});
			return;
		}

		this.cartItems = [...this.cartItems, { productAttribute, quantity: 1 }];
	}

	public removeProductAttribute(productAttribute: ProductAttribute) {
		const existingCartItem = this.cartItems.find(
			(cartItem) => cartItem.productAttribute.id === productAttribute.id,
		);

		if (existingCartItem?.quantity === 1) {
			this.cartItems = this.cartItems.filter(
				(cartItem) =>
					cartItem.productAttribute.id !== productAttribute.id,
			);
			return;
		}

		this.cartItems = this.cartItems.map((cartItem) => {
			if (cartItem.productAttribute.id === productAttribute.id) {
				return { ...cartItem, quantity: cartItem.quantity - 1 };
			}
			return cartItem;
		});
	}
}
