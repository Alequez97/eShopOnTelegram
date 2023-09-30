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

  private newLocalCart: CartItem[] = [];
  private localCart: { [key: number]: number } = {};

  constructor(productAttributes: ProductAttribute[]) {
    this.productAttributes = productAttributes;
    this.selectedProductAttribute = productAttributes[0];
    this.availableColors = [
      ...new Set(
        this.productAttributes
          .map((productAttribute) => productAttribute.color)
          .filter((color) => color !== undefined) as string[]
      ),
    ];
    this.availableSizes = [
      ...new Set(
        this.productAttributes
          .map((productAttribute) => productAttribute.size)
          .filter((color) => color !== undefined) as string[]
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
    if (this.selectedProductAttribute) {
      const productId = this.selectedProductAttribute.id;
      const amountOfSelectedProductAttributeAdded = this.localCart[productId];

      if (amountOfSelectedProductAttributeAdded === undefined) {
        this.localCart[productId] = 1;
      } else {
        if (
          amountOfSelectedProductAttributeAdded <
          this.selectedProductAttribute.quantityLeft
        ) {
          this.localCart[productId]++;
        }
      }
    }
  }

  public decreaseSelectedProductAttributeQuantity() {
    if (this.selectedProductAttribute) {
      const productId = this.selectedProductAttribute.id;
      this.localCart[productId]--;
    }
  }

  get getSelectedProductAttributeQuantityAddedToCart() {
    if (this.selectedProductAttribute) {
      return this.localCart[this.selectedProductAttribute.id] ?? 0;
    }

    return 0;
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
