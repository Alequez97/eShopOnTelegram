export interface ProductAttribute {
	id: number;
	color?: string;
	size?: string;
	originalPrice: number;
	priceWithDiscount?: number;
	quantityLeft: number;
	image: string;
}
