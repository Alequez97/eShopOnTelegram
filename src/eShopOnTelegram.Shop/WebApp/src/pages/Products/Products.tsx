import { ChangeEvent, useEffect, useState } from 'react';
import { Card } from '../../components/Card/Card';
import { Loader } from '../../components/Loader/Loader';
import { Error } from '../../components/Error/Error';
import { getCartItemsAsJsonString } from '../../utils/cart-items.utility';
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

export const Products = observer(() => {
	const telegramWebApp = useTelegramWebApp();

	useEffect(() => {
		telegramWebApp.expand();
		telegramWebApp.MainButton.setText('CHECKOUT');
	}, [telegramWebApp]);

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
		// const sendDataToTelegram = () => {
		// 	const json = getCartItemsAsJsonString(notEmptyCartItems);
		// 	telegramWebApp.sendData(json);
		// };

		const navigateToCheckout = () => navigate('checkout');

		if (notEmptyCartItems.length === 0) {
			telegramWebApp.MainButton.hide();
		} else {
			telegramWebApp.onEvent('mainButtonClicked', navigateToCheckout);
			telegramWebApp.MainButton.show();
		}

		return () => {
			telegramWebApp.offEvent('mainButtonClicked', navigateToCheckout);
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
