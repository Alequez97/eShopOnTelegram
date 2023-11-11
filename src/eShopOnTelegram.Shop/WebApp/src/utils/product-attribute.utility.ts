import { ProductAttribute } from '../types/product-attribute.type';

export const getPropertiesLabel = (productAttribute: ProductAttribute) => {
	if (!productAttribute.color && !productAttribute.size) {
		return undefined;
	}

	let label = '(';

	if (productAttribute.color) {
		label += productAttribute.color;

		if (productAttribute.size) {
			label += `, ${productAttribute.size}`;
		}
	}

	if (productAttribute.size) {
		label += productAttribute.size;
	}

	label += ')';

	return label;
};
