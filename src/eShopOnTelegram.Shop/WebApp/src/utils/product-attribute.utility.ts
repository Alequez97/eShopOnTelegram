import { ProductAttribute } from '../types/product-attribute.type';

export const getPropertiesLabel = (productAttribute: ProductAttribute) => {
	const { color, size } = productAttribute;

	if (!color && !size) {
		return undefined;
	}

	let label = '(';

	if (color) {
		label += color;

		if (size) {
			label += `, ${size}`;
		}
	} else if (size) {
		label += size;
	}

	label += ')';

	return label;
};
