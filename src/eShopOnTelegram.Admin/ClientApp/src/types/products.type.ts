export interface ProductAttribute {
  id: number;
  color?: string;
  size?: string;
  originalPrice: number;
  priceWithDiscount?: number;
  quantityLeft: number;
  image: string;
  productName: string;
  productCategoryName: string;
}

export interface Product {
  id: number;
  name: string;
  productCategoryName: string;
  productAttributes: ProductAttribute[];
}
