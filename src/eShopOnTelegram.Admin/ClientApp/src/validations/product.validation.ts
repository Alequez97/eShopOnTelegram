import { CreateProductAttributeRequest } from '../types/api.type';
import { FieldValues } from 'react-hook-form';

export const validatePriceWithDiscountShouldBeLessThanOriginalPrice = (
	_: CreateProductAttributeRequest,
	allValues: FieldValues,
) => {
	const hasValidationError = allValues.productAttributes.some(
		(productAttribute: CreateProductAttributeRequest) =>
			productAttribute.priceWithDiscount &&
			productAttribute?.priceWithDiscount >=
				productAttribute.originalPrice,
	);
	if (hasValidationError) {
		return 'Discounted price should be less than the original price';
	}
	return undefined; // Return undefined if the validation passes
};

// eslint-disable-next-line @typescript-eslint/no-explicit-any
export const validateFileExtension = (imageObject: any) => {
	const allowedExtensions = ['png', 'jpeg', 'jpg', 'gif'];

	const fileExtension = imageObject.title.split('.').pop().toLowerCase();
	if (!allowedExtensions.includes(fileExtension)) {
		return (
			'Invalid file format. Allowed formats are ' +
			allowedExtensions.join(', ')
		);
	}

	return undefined;
};
