import { observer } from 'mobx-react-lite';
import {
	StyledProductAttributeOptions,
	StyledProductAttributeSelectorWrapper,
} from './product-attribute-selector.styled';

interface ProductAttributeSelectorProps {
	productAttributeValues: string[] | null;
	selectedProductAttribute: string | null;
	onSelection: (productAttributeValue: string) => void;
}

export const ProductAttributeSelector = observer(
	({
		productAttributeValues,
		selectedProductAttribute,
		onSelection,
	}: ProductAttributeSelectorProps) => {
		return (
			<StyledProductAttributeSelectorWrapper
				$isVisible={
					productAttributeValues !== null &&
					productAttributeValues.length > 0
				}
			>
				{productAttributeValues?.map((productAttributeValue, index) => (
					<StyledProductAttributeOptions
						key={index}
						$isSelected={
							selectedProductAttribute === productAttributeValue
						}
						onClick={() => onSelection(productAttributeValue)}
					>
						{productAttributeValue}
					</StyledProductAttributeOptions>
				))}
			</StyledProductAttributeSelectorWrapper>
		);
	},
);
