export interface GetRequest {
  filter?: string;
  range?: string;
  sort?: string;
  paginationModel: PaginationModel;
}

export interface PaginationModel {
  from?: number;
  to?: number;
  sortPropertyName?: string;
  sortBy?: string;
}

export interface CreateProductRequest {
  name: string;
  productCategoryId: number;
  productAttributes: CreateProductAttributeRequest[];
}

export interface UpdateProductRequest {
  id: number;
  name: string;
  productAttributes: UpdateProductAttributeRequest[];
}

export interface CreateProductCategoryRequest {
  name: string;
}

export interface UpdateProductCategoryRequest {
  id: number;
  name: string;
}

export interface CreateProductAttributeRequest {
  color?: string;
  size?: string;
  originalPrice: number;
  priceWithDiscount?: number;
  quantityLeft: number;
  imageAsBase64: number[];
  imageName: string;
}

export interface UpdateProductAttributeRequest {
  id: number;
  color?: string;
  size?: string;
  originalPrice: number;
  priceWithDiscount?: number;
  quantityLeft: number;
  imageAsBase64?: number[];
  imageName?: string;
}

export interface CreateCartItemRequest {
  productAttributeId: number;
  quantity: number;
}

export interface CreateOrderRequest {
  telegramUserUID: number;
  cartItems: CreateCartItemRequest[];
}

export interface CreateCustomerRequest {
  telegramUserUID: number;
  username?: string;
  firstName: string;
  lastName?: string;
}

