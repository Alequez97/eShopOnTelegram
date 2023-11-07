import { ChangeEvent, useCallback, useEffect, useState } from 'react';
import { Card } from '../../components/Card/Card';
import { Loader } from '../../components/Loader/Loader';
import { Error } from '../../components/Error/Error';
import { getCartItemsAsJsonString } from '../../utils/cart-items.utility';
import { useCartItemsState } from '../../hooks/useCartItemsState';
import { useTelegramWebApp } from '../../hooks/useTelegramWebApp';
import { Product } from '../../types/product.type';
import { useProducts } from '../../hooks/useProducts';
import {
	StyledCardsContainer,
	StyledMissingProductsMessageWrapper,
	StyledProductCategoriesSelect,
	StyledProductCategoriesWrapper,
} from './products.styled';
import { ProductAttribute } from '../../types/product-attribute.type';

export const Products = () => {
	const telegramWebApp = useTelegramWebApp();
	useEffect(() => {
		telegramWebApp.expand();
	}, [telegramWebApp]);

	const {
		cartItems,
		addProductAttributeToState,
		removeProductAttributeFromState,
	} = useCartItemsState();

	const { products, productCategories, error, loading } = useProducts();
	const [filteredProducts, setFilteredProducts] = useState<
		Product[] | undefined
	>(undefined);

	useEffect(() => {
		const notEmptyCartItems = cartItems.filter(
			(cartItem) => cartItem.quantity > 0,
		);

		if (notEmptyCartItems.length === 0) {
			telegramWebApp.MainButton.hide();
		} else {
			telegramWebApp.MainButton.show();
		}
	}, [cartItems]);

	const sendDataToTelegram = useCallback(() => {
		const json = getCartItemsAsJsonString(cartItems);
		telegramWebApp.sendData(json);
	}, [cartItems]);

	useEffect(() => {
		telegramWebApp.onEvent('mainButtonClicked', sendDataToTelegram);
		return () => {
			telegramWebApp.offEvent('mainButtonClicked', sendDataToTelegram);
		};
	}, [sendDataToTelegram]);

	const onAdd = (productAttribute: ProductAttribute) => {
		addProductAttributeToState(productAttribute);
	};

	const onRemove = (productAttribute: ProductAttribute) => {
		removeProductAttributeFromState(productAttribute);
	};

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
			<h2 style={{ textAlign: 'center' }}>eShopOnTelegram</h2>

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
						<Card
							product={product}
							key={product.id}
							onAdd={onAdd}
							onRemove={onRemove}
						/>
					))}
				{filteredProducts !== undefined &&
					filteredProducts.map((product) => (
						<Card
							product={product}
							key={product.id}
							onAdd={onAdd}
							onRemove={onRemove}
						/>
					))}
			</StyledCardsContainer>
		</>
	);
};
