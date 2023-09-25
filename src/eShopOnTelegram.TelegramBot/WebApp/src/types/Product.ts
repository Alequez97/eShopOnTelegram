import { ProductAttribute } from "./productAttribute"

export interface Product {
    id: number
    name: string
    productCategoryName: string
    productAttributes: ProductAttribute[]
}