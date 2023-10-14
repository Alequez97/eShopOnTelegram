export const validatePriceWithDiscountShouldBeLessThanOriginalPrice = (
	value: any,
	allValues: any,
) => {
	if (value >= allValues.originalPrice) {
		return 'Discounted price should be less than the original price';
	}
	return undefined; // Return undefined if the validation passes
};

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
