import { Product } from '../../types/product.type';
import {
	StyledCard,
	StyledCardInfoWrapper,
	StyledCardPrice,
	StyledImageContainer,
} from './card.styled';
import { ProductAttributeSelector } from '../productAttributeSelector/productAttributeSelector';
import { observer } from 'mobx-react-lite';
import { CardStore } from './card.store';
import outOfStockImage from '../../assets/out_of_stock.jpg';
import { useCartItemsStore } from '../../contexts/cart-items-store.context';
import { useEffect, useState } from 'react';
import { Counter } from '../counter/Counter';
import { CartItem } from '../../types/cart-item.type';

interface CardProps {
	product: Product;
}

export const Card = observer(({ product }: CardProps) => {
	const cartItemsStore = useCartItemsStore();
	const [cardStore] = useState(new CardStore(product.productAttributes));

	const [
		selectedProductAttributeCartItem,
		setSelectedProductAttributeCartItem,
	] = useState<CartItem | undefined>(undefined);

	useEffect(() => {
		setSelectedProductAttributeCartItem(() => {
			const cartItem = cartItemsStore.cartItemsState.find(
				(cartItem) =>
					cartItem.productAttribute.id ===
					cardStore.selectedProductAttributeState?.id,
			);

			console.log({ ...cartItem });

			return cartItem;
		});
	}, [
		cardStore.selectedProductAttributeState,
		cartItemsStore.cartItemsState,
	]);

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
			<Counter
				value={selectedProductAttributeCartItem?.quantity}
				showAddButton={
					!selectedProductAttributeCartItem ||
					selectedProductAttributeCartItem.quantity === 0
				}
				addButtonDisabled={
					!cardStore.isAvailableSelectedProductAttribute
				}
				showPlusButton={
					!!selectedProductAttributeCartItem &&
					selectedProductAttributeCartItem?.quantity !== 0
				}
				showMinusButton={
					!!selectedProductAttributeCartItem &&
					selectedProductAttributeCartItem.quantity !== 0
				}
				onAdd={() => {
					if (cardStore.selectedProductAttributeState) {
						cartItemsStore.addProductAttribute(
							cardStore.selectedProductAttributeState,
						);
					}
				}}
				onRemove={() => {
					if (cardStore.selectedProductAttributeState) {
						cartItemsStore.removeProductAttribute(
							cardStore.selectedProductAttributeState,
						);
					}
				}}
			/>
		</StyledCard>
	);
});
