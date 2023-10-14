export interface CreateProductRequest {
	name: string;
	productCategoryId: number;
	productAttributes: CreateProductAttributeRequest[];
}

export interface CreateProductAttributeRequest {
	color?: string | null;
	size?: string | null;
	originalPrice: number;
	priceWithDiscount?: number | null;
	quantityLeft: number;
	imageAsBase64: string;
	imageName: string;
}

export interface UpdateProductRequest {
	id: number;
	name: string;
	productAttributes: UpdateProductAttributeRequest[];
}

export interface UpdateProductAttributeRequest {
	id: number;
	color?: string | null;
	size?: string | null;
	originalPrice: number;
	priceWithDiscount?: number | null;
	quantityLeft: number;
	imageAsBase64?: string | null;
	imageName?: string | null;
}
