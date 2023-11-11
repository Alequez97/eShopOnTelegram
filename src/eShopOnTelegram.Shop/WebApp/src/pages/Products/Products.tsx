import { ChangeEvent, useEffect, useState } from 'react';
import { Card } from '../../components/Card/Card';
import { Loader } from '../../components/Loader/Loader';
import { Error } from '../../components/Error/Error';
import { useTelegramWebApp } from '../../hooks/useTelegramWebApp';
import { Product } from '../../types/product.type';
import { useProducts } from '../../hooks/useProducts';
import {
	StyledCardsContainer,
	StyledMissingProductsMessageWrapper,
	StyledProductCategoriesSelect,
	StyledProductCategoriesWrapper,
} from './products.styled';
import { useCartItemsStore } from '../../contexts/cart-items-store.context';
import { observer } from 'mobx-react-lite';
import { useNavigate } from 'react-router-dom';
import { RouteLocation } from '../../enums/route-location.enum';

export const Products = observer(() => {
	const telegramWebApp = useTelegramWebApp();
	telegramWebApp.MainButton.setText('CHECKOUT');

	const { products, productCategories, error, loading } = useProducts();
	const [filteredProducts, setFilteredProducts] = useState<
		Product[] | undefined
	>(undefined);

	const cartItemsStore = useCartItemsStore();

	const navigate = useNavigate();

	useEffect(() => {
		const notEmptyCartItems = cartItemsStore.cartItemsState.filter(
			(cartItem) => cartItem.quantity > 0,
		);
		const navigateToCheckout = () => navigate(RouteLocation.CHECKOUT);

		if (notEmptyCartItems.length === 0) {
			telegramWebApp.MainButton.hide();
		} else {
			telegramWebApp.MainButton.onClick(navigateToCheckout);
			telegramWebApp.MainButton.show();
		}

		return () => {
			telegramWebApp.MainButton.offClick(navigateToCheckout);
		};
	}, [cartItemsStore.cartItemsState]);

	if (loading) {
		return <Loader />;
	}

	if (error) {
		return <Error />;
	}

	const DEFAULT_SELECTOR_VALUE = 'All categories';
	const selectOnChangeHandler = (event: ChangeEvent<HTMLSelectElement>) => {
		const selectedOption = event.target.value;
		if (selectedOption === DEFAULT_SELECTOR_VALUE) {
			setFilteredProducts(undefined);
			return;
		}

		setFilteredProducts(
			products.filter(
				(product) => product.productCategoryName === selectedOption,
			),
		);
	};

	return (
		<>
			{products.length === 0 && (
				<StyledMissingProductsMessageWrapper>
					<span>No available products at this moment</span>
				</StyledMissingProductsMessageWrapper>
			)}

			{products.length !== 0 && (
				<StyledProductCategoriesWrapper>
					<StyledProductCategoriesSelect
						name="product-categories"
						defaultValue={DEFAULT_SELECTOR_VALUE}
						onChange={selectOnChangeHandler}
					>
						<option
							value={DEFAULT_SELECTOR_VALUE}
							key={DEFAULT_SELECTOR_VALUE}
						>
							{DEFAULT_SELECTOR_VALUE}
						</option>
						{productCategories?.map((category) => (
							<option value={category} key={category}>
								{category}
							</option>
						))}
					</StyledProductCategoriesSelect>
				</StyledProductCategoriesWrapper>
			)}

			<StyledCardsContainer>
				{filteredProducts === undefined &&
					products.map((product) => (
						<Card product={product} key={product.id} />
					))}
				{filteredProducts !== undefined &&
					filteredProducts.map((product) => (
						<Card product={product} key={product.id} />
					))}
			</StyledCardsContainer>
		</>
	);
});
