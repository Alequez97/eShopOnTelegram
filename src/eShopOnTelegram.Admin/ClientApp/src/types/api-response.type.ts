export interface Base {
	id: number;
}

export interface Product {
	name: string;
	productCategoryName: string;
	productAttributes: ProductAttribute[];
	id: number;
}

export interface ProductCategory {
	name: string;
	id: number;
}

export interface ProductAttribute {
	color?: string;
	size?: string;
	productName: string;
	productCategoryName: string;
	originalPrice: number;
	priceWithDiscount?: number;
	quantityLeft: number;
	image?: string;
	id: number;
}

export interface CartItem {
	productAttribute: ProductAttribute;
	quantity: number;
	totalPrice: number;
}

export interface Order {
	orderNumber: string;
	customerId: number;
	telegramUserUID: number;
	username?: string;
	firstName: string;
	lastName?: string;
	cartItems: CartItem[];
	creationDate: Date;
	paymentDate?: Date;
	status: string;
	country?: string;
	city?: string;
	streetLine1?: string;
	streetLine2?: string;
	postCode?: string;
	totalPrice: number;
	id: number;
}

export interface Customer {
	telegramUserUID: number;
	username?: string;
	firstName: string;
	lastName?: string;
	id: number;
}
