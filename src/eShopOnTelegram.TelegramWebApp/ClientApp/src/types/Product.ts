interface Product {
    id: number
    productName: string
    productCategoryName: string
    originalPrice: number
    priceWithDiscount?: number
    quantityLeft: number,
    image: string
}

export default Product