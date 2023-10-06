import { ProductAttribute } from "./products.type"

export interface CartItem {
    productAttribute: ProductAttribute
    quantity: number
}