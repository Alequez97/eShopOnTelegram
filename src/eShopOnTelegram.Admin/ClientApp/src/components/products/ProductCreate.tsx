import {
	ArrayInput,
	FileInput,
	NumberInput,
	required,
	SelectInput,
	SimpleForm,
	SimpleFormIterator,
	TextInput,
	useNotify,
	useRedirect,
} from 'react-admin';
import { replaceEmptyKeysWithNull } from '../../utils/object.utility';
import { fileToBase64 } from '../../utils/file.utility';
import {
	validateFileExtension,
	validatePriceWithDiscountShouldBeLessThanOriginalPrice,
} from '../../validations/product.validation';
import { axiosGet, axiosPost } from '../../utils/axios.utility';
import { useEffect, useState } from 'react';
import { FieldValues } from 'react-hook-form';

function ProductCreate() {
	const notify = useNotify();
	const redirect = useRedirect();

	const handleProductCreate = async (request: FieldValues) => {
		try {
			for (
				let index = 0;
				index < request.productAttributes.length;
				index++
			) {
				const productAttribute = request.productAttributes[index];

				request.productAttributes[index].imageAsBase64 =
					await fileToBase64(productAttribute.productImage.rawFile);
				request.productAttributes[index].imageName =
					productAttribute.productImage.rawFile.name;

				await replaceEmptyKeysWithNull(
					request.productAttributes[index],
				);
			}
			await axiosPost('/products', request);
			notify('New product created', { type: 'success' });
			redirect('/products');
		} catch (error) {
			notify('Error saving application content data', { type: 'error' });
		}
	};

	const [productCategories, setProductCategories] = useState(null);
	const getProductCategories = async () => {
		return await axiosGet('/productCategories');
	};

	useEffect(() => {
		getProductCategories().then((productCategories) =>
			setProductCategories(productCategories),
		);
	}, []);

	if (!productCategories) {
		return <div>Loading...</div>;
	}

	return (
		<SimpleForm onSubmit={handleProductCreate}>
			<SelectInput
				optionText="name"
				validate={[required()]}
				source={'productCategoryId'}
				optionValue="id"
				choices={productCategories}
			/>
			<TextInput
				source="name"
				label="Product name"
				validate={[required()]}
			/>
			<div>
				<p>
					We support adding color and size as additional properties
					for your products. If your product does not require this
					additional information just leave it empty
				</p>
			</div>
			<ArrayInput
				source="productAttributes"
				validate={[
					required('At least one product attribute is required'),
				]}
			>
				<SimpleFormIterator inline disableReordering>
					<NumberInput
						source="originalPrice"
						label="Original Price"
						validate={[required('Original price is required')]}
					/>
					<NumberInput
						source="priceWithDiscount"
						label="Price With Discount"
						defaultValue={null}
						validate={[
							validatePriceWithDiscountShouldBeLessThanOriginalPrice,
						]}
					/>
					<NumberInput
						source="quantityLeft"
						label="Quantity Left"
						validate={[required('Quantity is required')]}
					/>
					<TextInput source="color" label="Color" />
					<TextInput source="size" label="Size" />
					<FileInput
						source="productImage"
						label="Product Image"
						accept="image/*"
						validate={[
							required('Image is required'),
							validateFileExtension,
						]}
					/>
				</SimpleFormIterator>
			</ArrayInput>
		</SimpleForm>
	);
}

export default ProductCreate;
