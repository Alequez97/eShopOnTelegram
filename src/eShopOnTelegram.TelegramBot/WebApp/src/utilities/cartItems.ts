import ICartItem from "../types/cart-item";

export function getCartItemsAsJsonString(cartItems: ICartItem[]) {
    let products: string[] = [];

    cartItems.forEach(cartItem => {
        products.push(`{"productId":"${cartItem.product.id}", "productName":"${cartItem.product.name}", "originalPrice":${cartItem.product.originalPrice}, "priceWithDiscount":${cartItem.product?.priceWithDiscount}, "quantity":${cartItem.quantity}}`);
    })

    return `{"cartItems":[${products.join(',')}]}`;
}