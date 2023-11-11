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
import { ProductAttributeSelector } from '../productAttributeSelector/productAttributeSelector';
import { observer } from 'mobx-react-lite';
import { CardStore } from './card.store';
import outOfStockImage from '../../assets/out_of_stock.jpg';
import { useCartItemsStore } from '../../contexts/cart-items-store.context';
import { useState } from 'react';

interface CardProps {
	product: Product;
}

export const Card = observer(({ product }: CardProps) => {
	const cartItemsStore = useCartItemsStore();
	const [cardStore] = useState(new CardStore(product.productAttributes));

	const selectedProductAttributeCartItem = cartItemsStore.cartItemsState.find(
		(cartItem) =>
			cartItem.productAttribute.id ===
			cardStore.selectedProductAttributeState?.id,
	);

	return (
		<StyledCard>
			<StyledImageContainer>
				{cardStore.selectedProductAttributeState ? (
					<img
						src={cardStore.selectedProductAttributeState.image}
						alt={product.name}
					/>
				) : (
					<img src={outOfStockImage} alt={'Out of stock'} />
				)}
			</StyledImageContainer>
			<StyledCardInfoWrapper>
				{product.name}
				<br />
				<StyledCardPrice>
					{cardStore.selectedProductAttributeState && (
						<>
							{
								cardStore.selectedProductAttributeState
									.originalPrice
							}{' '}
							€
						</>
					)}
				</StyledCardPrice>
				<br />
				{cardStore.selectedProductAttributeState ? (
					<i>
						Available:{' '}
						{cardStore.selectedProductAttributeState.quantityLeft <
						20
							? cardStore.selectedProductAttributeState
									.quantityLeft
							: '20+'}
					</i>
				) : (
					<i>Available: 0</i>
				)}
				<ProductAttributeSelector
					productAttributeValues={cardStore.availableColorsState}
					selectedProductAttribute={cardStore.selectedColorState}
					onSelection={(color: string) =>
						cardStore.setSelectedColor(color)
					}
				/>
				<ProductAttributeSelector
					productAttributeValues={cardStore.availableSizesState}
					selectedProductAttribute={cardStore.selectedSizeState}
					onSelection={(color: string) =>
						cardStore.setSelectedSize(color)
					}
				/>
			</StyledCardInfoWrapper>

			<StyledButtonContainer>
				{(!selectedProductAttributeCartItem ||
					selectedProductAttributeCartItem.quantity === 0) && (
					<Button
						title={'Add'}
						type={'add'}
						onClick={() => {
							if (cardStore.selectedProductAttributeState) {
								cartItemsStore.addProductAttribute(
									cardStore.selectedProductAttributeState,
								);
							}
						}}
						disabled={!cardStore.hasSelectedProductAttribute}
					/>
				)}
				{selectedProductAttributeCartItem &&
					selectedProductAttributeCartItem?.quantity !== 0 && (
						<Button
							title={'-'}
							type={'remove'}
							onClick={() => {
								if (cardStore.selectedProductAttributeState) {
									cartItemsStore.removeProductAttribute(
										cardStore.selectedProductAttributeState,
									);
								}
							}}
							disabled={false}
						/>
					)}
				<StyledCardBadge
					$isVisible={
						selectedProductAttributeCartItem !== undefined &&
						selectedProductAttributeCartItem.quantity !== 0
					}
				>
					{selectedProductAttributeCartItem?.quantity}
				</StyledCardBadge>
				{selectedProductAttributeCartItem &&
					selectedProductAttributeCartItem.quantity !== 0 && (
						<Button
							title={'+'}
							type={'add'}
							onClick={() => {
								if (cardStore.selectedProductAttributeState) {
									cartItemsStore.addProductAttribute(
										cardStore.selectedProductAttributeState,
									);
								}
							}}
							disabled={false}
						/>
					)}
			</StyledButtonContainer>
		</StyledCard>
	);
});
