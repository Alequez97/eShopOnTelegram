import { makeAutoObservable } from 'mobx';
import { ProductAttribute } from '../../types/product-attribute.type';

export class CardStore {
	private readonly productAttributes: ProductAttribute[];
	private selectedProductAttribute: ProductAttribute | undefined;

	private selectedColor: string | null;
	private selectedSize: string | null;

	private availableColors: string[];
	private availableSizes: string[];

	constructor(productAttributes: ProductAttribute[]) {
		this.productAttributes = productAttributes;
		this.selectedProductAttribute = productAttributes[0];
		this.availableColors = [
			...new Set(
				this.productAttributes
					.map((productAttribute) => productAttribute.color)
					.filter(
						(color) => color !== undefined && color !== null,
					) as string[],
			),
		];
		this.availableSizes = [
			...new Set(
				this.productAttributes
					.map((productAttribute) => productAttribute.size)
					.filter(
						(size) => size !== undefined && size !== null,
					) as string[],
			),
		];
		this.selectedColor = this.availableColors[0] ?? null;
		this.selectedSize = this.availableSizes[0] ?? null;

		makeAutoObservable(this);
	}

	get selectedProductAttributeState() {
		return this.selectedProductAttribute;
	}

	get availableColorsState() {
		return this.availableColors;
	}

	get availableSizesState() {
		return this.availableSizes;
	}

	get hasSelectedProductAttribute() {
		return (
			this.selectedProductAttribute !== undefined &&
			this.selectedProductAttribute.quantityLeft > 0
		);
	}

	public setSelectedColor(color: string) {
		const productAttribute = this.productAttributes.find(
			(productAttribute) => productAttribute.color === color,
		);

		if (productAttribute) {
			this.selectedColor = color;
			this.updateSelectedProductAttribute();
		}
	}

	get selectedColorState() {
		return this.selectedColor;
	}

	public setSelectedSize(size: string) {
		const productAttribute = this.productAttributes.find(
			(productAttribute) => productAttribute.size === size,
		);

		if (productAttribute) {
			this.selectedSize = size;
			this.updateSelectedProductAttribute();
		}
	}

	get selectedSizeState() {
		return this.selectedSize;
	}

	private get colorIsRequired() {
		return (
			this.productAttributes.find(
				(productAttribute) => productAttribute.color !== undefined,
			) !== undefined
		);
	}

	private get sizeIsRequired() {
		return (
			this.productAttributes.find(
				(productAttribute) => productAttribute.size !== undefined,
			) !== undefined
		);
	}

	private updateSelectedProductAttribute() {
		if (this.colorIsRequired && this.sizeIsRequired) {
			this.selectedProductAttribute = this.productAttributes.find(
				(productAttribute) =>
					productAttribute.color === this.selectedColor &&
					productAttribute.size === this.selectedSize,
			) as ProductAttribute;
		} else if (this.colorIsRequired) {
			this.selectedProductAttribute = this.productAttributes.find(
				(productAttribute) =>
					productAttribute.color === this.selectedColor,
			) as ProductAttribute;
		} else if (this.sizeIsRequired) {
			this.selectedProductAttribute = this.productAttributes.find(
				(productAttribute) =>
					productAttribute.size === this.selectedSize,
			) as ProductAttribute;
		}
	}
}
