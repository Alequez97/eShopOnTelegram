import { ChangeEvent, useState } from 'react';
import { Card } from '../../components/Card/Card';
import { Loader } from '../../components/Loader/Loader';
import { Error } from '../../components/Error/Error';
import { Product } from '../../types/product.type';
import { useProducts } from '../../hooks/useProducts';
import {
	StyledCardsContainer,
	StyledMissingProductsMessageWrapper,
	StyledProductCategoriesSelect,
	StyledProductCategoriesWrapper,
} from './page-products.styled';
import { useCartItemsStore } from '../../contexts/cart-items-store.context';
import { observer } from 'mobx-react-lite';
import { useNavigate } from 'react-router-dom';
import { RouteLocation } from '../../enums/route-location.enum';
import { useTranslations } from '../../contexts/translations.context';
import { useTelegramMainButton } from '../../hooks/telegram/useTelegramMainButton';

export const PageProducts = observer(() => {
	const translations = useTranslations();

	const { products, productCategories, error, loading } = useProducts();
	const [filteredProducts, setFilteredProducts] = useState<
		Product[] | undefined
	>(undefined);

	const cartItemsStore = useCartItemsStore();
	const navigate = useNavigate();

	const notEmptyCartItems = cartItemsStore.cartItemsState.filter(
		(cartItem) => cartItem.quantity > 0,
	);
	const navigateToCheckout = () => {
		cartItemsStore.removeEmptyCartItems();
		navigate(RouteLocation.CHECKOUT);
	};

	useTelegramMainButton(
		notEmptyCartItems.length > 0,
		navigateToCheckout,
		translations.continue.toUpperCase(),
	);

	if (loading) {
		return <Loader />;
	}

	if (error) {
		return <Error />;
	}

	const DEFAULT_SELECTOR_VALUE = translations.allCategories;

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
					<span>{translations.noAvailableProducts}</span>
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
