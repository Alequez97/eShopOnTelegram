import { useState } from 'react';
import { Button } from '../Button/Button';
import { Product } from '../../types/product.type';
import {
	StyledButtonContainer,
	StyledCard,
	StyledCardBadge,
	StyledCardInfoWrapper,
	StyledCardPrice,
	StyledImageContainer,
} from './card.styled';
import { ProductAttribute } from '../../types/product-attribute.type';
import { ProductAttributeSelector } from '../productAttributeSelector/productAttributeSelector';
import { observer } from 'mobx-react-lite';
import { CardStore } from './card.store';
import outOfStockImage from '../../assets/out_of_stock.jpg';

interface CardProps {
	product: Product;
	onAdd: (productAttribute: ProductAttribute) => void;
	onRemove: (productAttribute: ProductAttribute) => void;
}

export const Card = observer(({ product, onAdd, onRemove }: CardProps) => {
	const [cardStore] = useState(new CardStore(product.productAttributes));
	const { name } = product;

	return (
		<StyledCard>
			<StyledImageContainer>
				{cardStore.getSelectedProductAttribute ? (
					<img
						src={cardStore.getSelectedProductAttribute.image}
						alt={name}
					/>
				) : (
					<img src={outOfStockImage} alt={'Out of stock'} />
				)}
			</StyledImageContainer>
			<StyledCardInfoWrapper>
				{name}
				<br />
				<StyledCardPrice>
					{cardStore.getSelectedProductAttribute && (
						<>
							{
								cardStore.getSelectedProductAttribute
									.originalPrice
							}{' '}
							€
						</>
					)}
				</StyledCardPrice>
				<br />
				{cardStore.getSelectedProductAttribute ? (
					<i>
						Available:{' '}
						{cardStore.getSelectedProductAttribute.quantityLeft < 20
							? cardStore.getSelectedProductAttribute.quantityLeft
							: '20+'}
					</i>
				) : (
					<i>Available: 0</i>
				)}
				<ProductAttributeSelector
					productAttributeValues={cardStore.getAvailableColors}
					selectedProductAttribute={cardStore.getSelectedColor}
					onSelection={(color: string) =>
						cardStore.setSelectedColor(color)
					}
				/>
				<ProductAttributeSelector
					productAttributeValues={cardStore.getAvailableSizes}
					selectedProductAttribute={cardStore.getSelectedSize}
					onSelection={(color: string) =>
						cardStore.setSelectedSize(color)
					}
				/>
			</StyledCardInfoWrapper>

			<StyledButtonContainer>
				{cardStore.getSelectedProductAttributeQuantityAddedToCart ===
					0 && (
					<Button
						title={'Add'}
						type={'add'}
						onClick={() => {
							cardStore.increaseSelectedProductAttributeQuantity();
							onAdd(cardStore.getSelectedProductAttribute);
						}}
						disabled={!cardStore.selectionStateIsValid}
					/>
				)}
				{cardStore.getSelectedProductAttributeQuantityAddedToCart !==
					0 && (
					<Button
						title={'-'}
						type={'remove'}
						onClick={() => {
							cardStore.decreaseSelectedProductAttributeQuantity();
							onRemove(cardStore.getSelectedProductAttribute);
						}}
						disabled={false}
					/>
				)}
				<StyledCardBadge
					$isVisible={
						cardStore.getSelectedProductAttributeQuantityAddedToCart !==
						0
					}
				>
					{cardStore.getSelectedProductAttributeQuantityAddedToCart}
				</StyledCardBadge>
				{cardStore.getSelectedProductAttributeQuantityAddedToCart !==
					0 && (
					<Button
						title={'+'}
						type={'add'}
						onClick={() => {
							cardStore.increaseSelectedProductAttributeQuantity();
							onAdd(cardStore.getSelectedProductAttribute);
						}}
						disabled={false}
					/>
				)}
			</StyledButtonContainer>
		</StyledCard>
	);
});
