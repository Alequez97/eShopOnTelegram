import { makeAutoObservable } from "mobx";
import { ProductAttribute } from "../../types/productAttribute";
import { CartItem } from "../../types/cart-item";

export class CardStore {
  private readonly productAttributes: ProductAttribute[];
  private selectedProductAttribute: ProductAttribute;

  private selectedColor: string | null;
  private selectedSize: string | null;

  private availableColors: string[];
  private availableSizes: string[];

  private localCartItems: CartItem[] = [];

  constructor(productAttributes: ProductAttribute[]) {
    this.productAttributes = productAttributes;
    this.selectedProductAttribute = productAttributes[0];
    this.availableColors = [
      ...new Set(
        this.productAttributes
          .map((productAttribute) => productAttribute.color)
          .filter((color) => color !== undefined && color !== null) as string[]
      ),
    ];
    this.availableSizes = [
      ...new Set(
        this.productAttributes
          .map((productAttribute) => productAttribute.size)
          .filter((size) => size !== undefined && size !== null) as string[]
      ),
    ];
    this.selectedColor = this.availableColors[0] ?? null;
    this.selectedSize = this.availableSizes[0] ?? null;

    makeAutoObservable(this);
  }

  get getSelectedProductAttribute() {
    return this.selectedProductAttribute;
  }

  get getAvailableColors() {
    return this.availableColors;
  }

  get getAvailableSizes() {
    return this.availableSizes;
  }

  get selectionStateIsValid() {
    return (
      this.selectedProductAttribute !== undefined &&
      this.selectedProductAttribute.quantityLeft > 0
    );
  }

  public setSelectedColor(color: string) {
    const productAttribute = this.productAttributes.find(
      (productAttribute) => productAttribute.color === color
    );

    if (productAttribute) {
      this.selectedColor = color;
      this.updateSelectedProductAttribute();
    }
  }

  get getSelectedColor() {
    return this.selectedColor;
  }

  public setSelectedSize(size: string) {
    const productAttribute = this.productAttributes.find(
      (productAttribute) => productAttribute.size === size
    );

    if (productAttribute) {
      this.selectedSize = size;
      this.updateSelectedProductAttribute();
    }
  }

  get getSelectedSize() {
    return this.selectedSize;
  }

  public increaseSelectedProductAttributeQuantity() {
    if (!this.selectedProductAttribute) {
      return;
    }

    const selectedProductAttributeCartItem = this.localCartItems.find(
      (cartItem) =>
        cartItem.productAttribute.id === this.selectedProductAttribute.id
    );

    if (!selectedProductAttributeCartItem) {
      this.localCartItems.push({
        productAttribute: this.selectedProductAttribute,
        quantity: 1,
      });
    } else {
      selectedProductAttributeCartItem.quantity++;
    }
  }

  public decreaseSelectedProductAttributeQuantity() {
    if (!this.selectedProductAttribute) {
      return;
    }

    const selectedProductAttributeCartItem = this.localCartItems.find(
      (cartItem) =>
        cartItem.productAttribute.id === this.selectedProductAttribute.id
    );
    if (selectedProductAttributeCartItem) {
      selectedProductAttributeCartItem.quantity--;
    }
  }

  get getSelectedProductAttributeQuantityAddedToCart() {
    if (!this.selectedProductAttribute) {
      return 0;
    }

    const localCartItem = this.localCartItems.find(
      (cartItem) =>
        cartItem.productAttribute.id === this.selectedProductAttribute.id
    );

    if (!localCartItem) {
      return 0;
    }

    return localCartItem.quantity;
  }

  get getLocalCartItems() {
    return this.localCartItems;
  }

  private get colorIsRequired() {
    return (
      this.productAttributes.find(
        (productAttribute) => productAttribute.color !== undefined
      ) !== undefined
    );
  }

  private get sizeIsRequired() {
    return (
      this.productAttributes.find(
        (productAttribute) => productAttribute.size !== undefined
      ) !== undefined
    );
  }

  private updateSelectedProductAttribute() {
    if (this.colorIsRequired && this.sizeIsRequired) {
      const productAttribute = this.productAttributes.find(
        (productAttribute) =>
          productAttribute.color === this.selectedColor &&
          productAttribute.size === this.selectedSize
      ) as ProductAttribute;
      this.selectedProductAttribute = productAttribute;
    } else if (this.colorIsRequired) {
      this.selectedProductAttribute = this.productAttributes.find(
        (productAttribute) => productAttribute.color === this.selectedColor
      ) as ProductAttribute;
    } else if (this.sizeIsRequired) {
      this.selectedProductAttribute = this.productAttributes.find(
        (productAttribute) => productAttribute.size === this.selectedSize
      ) as ProductAttribute;
    }
  }
}
