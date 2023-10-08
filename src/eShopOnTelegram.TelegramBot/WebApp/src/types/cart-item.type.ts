import { ProductAttribute } from "./product-attribute.type";

export interface CartItem {
    productAttribute: ProductAttribute
    quantity: number
}
