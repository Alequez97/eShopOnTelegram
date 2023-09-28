import { makeAutoObservable } from "mobx";
import { ProductAttribute } from "../../types/productAttribute";

export class CardStore {
  private readonly productAttributes: ProductAttribute[];
  private selectedProductAttribute: ProductAttribute;
  private selectedColor: string | null;
  private selectedSize: string | null;

  constructor(productAttributes: ProductAttribute[]) {
    this.productAttributes = productAttributes;
    this.selectedProductAttribute = productAttributes[0];
    this.selectedColor = null;
    this.selectedSize = null;

    makeAutoObservable(this);
  }

  get getSelectedProductAttribute() {
    return this.selectedProductAttribute;
  }

  get selectionStateIsValid() {
    return this.colorSetOrNotRequired && this.sizeSetOrNotRequired;
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

  public setSelectedSize(size: string) {
    const productAttribute = this.productAttributes.find(
      (productAttribute) => productAttribute.size === size
    );

    if (productAttribute) {
      this.selectedSize = size;
      this.updateSelectedProductAttribute();
    }
  }

  private get colorIsRequired() {
    return (
      this.productAttributes.find(
        (productAttribute) => productAttribute.color !== undefined
      ) !== undefined
    );
  }

  private get colorSetOrNotRequired() {
    if (this.colorIsRequired) {
      return this.selectedColor !== null;
    }

    return true;
  }

  private get sizeIsRequired() {
    return (
      this.productAttributes.find(
        (productAttribute) => productAttribute.size !== undefined
      ) !== undefined
    );
  }

  private get sizeSetOrNotRequired() {
    if (this.sizeIsRequired) {
      return this.selectedSize !== null;
    }

    return true;
  }

  private updateSelectedProductAttribute() {
    if (this.colorIsRequired && this.sizeIsRequired) {
      if (this.colorSetOrNotRequired && this.sizeSetOrNotRequired) {
        const productAttribute = this.productAttributes.find(
          (productAttribute) =>
            productAttribute.color === this.selectedColor &&
            productAttribute.size === this.selectedSize
        ) as ProductAttribute;
        this.selectedProductAttribute = productAttribute;
      }
    } else if (this.colorIsRequired && this.colorSetOrNotRequired) {
      this.selectedProductAttribute = this.productAttributes.find(
        (productAttribute) => productAttribute.color === this.selectedColor
      ) as ProductAttribute;
    } else if (this.sizeIsRequired && this.sizeSetOrNotRequired) {
      this.selectedProductAttribute = this.productAttributes.find(
        (productAttribute) => productAttribute.size === this.selectedSize
      ) as ProductAttribute;
    }
  }
}
