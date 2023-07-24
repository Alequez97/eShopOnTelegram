interface Product {
    id: number
    name: string
    productCategoryName: string
    originalPrice: number
    priceWithDiscount?: number
    quantityLeft: number,
    image: string
}

export default Product