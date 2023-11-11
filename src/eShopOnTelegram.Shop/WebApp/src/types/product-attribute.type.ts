export interface ProductAttribute {
	id: number;
	productName: string;
	productCategoryName: string;
	color?: string;
	size?: string;
	originalPrice: number;
	priceWithDiscount?: number;
	quantityLeft: number;
	image: string;
}
