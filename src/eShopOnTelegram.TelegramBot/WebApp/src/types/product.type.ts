import { ProductAttribute } from "./product-attribute.type"

export interface Product {
    id: number
    name: string
    productCategoryName: string
    productAttributes: ProductAttribute[]
}