import React from 'react';
import {
	ArrayInput,
	Edit,
	FileInput,
	NumberInput,
	required,
	SimpleForm,
	SimpleFormIterator,
	TextInput,
	useNotify,
	useRedirect,
} from 'react-admin';
import { validatePriceWithDiscountShouldBeLessThanOriginalPrice } from '../../validations/product.validation';
import { fileToBase64 } from '../../utils/file.utility';
import { replaceEmptyKeysWithNull } from '../../utils/object.utility';
import axios from 'axios';
import { FieldValues } from 'react-hook-form';

export function ProductEdit() {
	const notify = useNotify();
	const redirect = useRedirect();

	const handleProductUpdate = async (request: FieldValues) => {
		try {
			for (
				let index = 0;
				index < request.productAttributes.length;
				index++
			) {
				const productAttribute = request.productAttributes[index];

				if (productAttribute?.productImage?.rawFile) {
					request.productAttributes[index].imageAsBase64 =
						await fileToBase64(
							productAttribute.productImage.rawFile,
						);

					request.productAttributes[index].imageName =
						productAttribute.productImage.rawFile.name;
				}

				await replaceEmptyKeysWithNull(
					request.productAttributes[index],
				);
			}
			await axios.put(`/products/${request.id}`, request);
			notify('Product updated', { type: 'info' });
			redirect('/products');
		} catch (error) {
			console.error(error);
			notify('Error saving application content data', { type: 'error' });
		}
	};

	return (
		<Edit title="Edit product">
			<SimpleForm onSubmit={handleProductUpdate}>
				<TextInput disabled source="id" />
				<TextInput source="name" validate={required()} />
				<ArrayInput
					source="productAttributes"
					label="Product Attributes"
					validate={[
						required('At least one product attribute is required'),
					]}
				>
					<SimpleFormIterator inline disableReordering>
						<NumberInput
							source="originalPrice"
							label="Original Price"
						/>
						<NumberInput
							source="priceWithDiscount"
							label="Price With Discount"
							validate={[
								validatePriceWithDiscountShouldBeLessThanOriginalPrice,
							]}
						/>
						<NumberInput
							source="quantityLeft"
							label="Quantity Left"
							validate={required()}
						/>
						<TextInput source="color" label="Color" />
						<TextInput source="size" label="Size" />
						<FileInput
							source="productImage"
							label="Product Image"
							accept="image/*"
						/>
					</SimpleFormIterator>
				</ArrayInput>
			</SimpleForm>
		</Edit>
	);
}
