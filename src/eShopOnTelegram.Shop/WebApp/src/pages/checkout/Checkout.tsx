import { useCartItemsStore } from '../../contexts/cart-items-store.context';
import { useTelegramWebApp } from '../../hooks/useTelegramWebApp';
import { useEffect } from 'react';
import { getCartItemsAsJsonString } from '../../utils/cart-items.utility';
import { useNavigate } from 'react-router-dom';
import { RouteLocation } from '../../enums/route-location.enum';
import {
	StyledCheckoutPageCartItemContainer,
	StyledCheckoutPageContainer,
	StyledCheckoutPageBoldText,
} from './checkout.styled';
import { getPropertiesLabel } from '../../utils/product-attribute.utility';

export const Checkout = () => {
	const navigate = useNavigate();
	const telegramWebApp = useTelegramWebApp();
	telegramWebApp.MainButton.setText('PROCEED TO PAYMENT');

	useEffect(() => {
		telegramWebApp.BackButton?.show();

		const navigateToProducts = () => navigate(RouteLocation.PRODUCTS);

		telegramWebApp.BackButton?.onClick(navigateToProducts);

		return () => {
			telegramWebApp.BackButton?.hide();
			telegramWebApp.BackButton?.offClick(navigateToProducts);
		};
	}, []);

	const cartItemsStore = useCartItemsStore();

	useEffect(() => {
		const notEmptyCartItems = cartItemsStore.cartItemsState.filter(
			(cartItem) => cartItem.quantity > 0,
		);

		const sendDataToTelegram = () => {
			const json = getCartItemsAsJsonString(notEmptyCartItems);
			telegramWebApp.sendData(json);
		};

		if (notEmptyCartItems.length === 0) {
			telegramWebApp.MainButton.hide();
		} else {
			telegramWebApp.MainButton.onClick(sendDataToTelegram);
			telegramWebApp.MainButton.show();
		}

		return () => {
			telegramWebApp.MainButton.offClick(sendDataToTelegram);
		};
	}, [cartItemsStore.cartItemsState]);

	return (
		<StyledCheckoutPageContainer>
			{cartItemsStore.cartItemsState.map((cartItem) => {
				const { productAttribute } = cartItem;

				return (
					<StyledCheckoutPageCartItemContainer
						key={productAttribute.id}
						justifyContent={'space-between'}
						hasBorder={true}
					>
						<div>
							<div>
								<StyledCheckoutPageBoldText>
									{productAttribute.productName}
								</StyledCheckoutPageBoldText>{' '}
								{getPropertiesLabel(productAttribute)}{' '}
							</div>
							<div>
								{productAttribute.priceWithDiscount
									? productAttribute.priceWithDiscount
									: productAttribute.originalPrice}
							</div>
						</div>
						<div>
							<button type={'button'}>+</button>
						</div>
					</StyledCheckoutPageCartItemContainer>
				);
			})}
			<StyledCheckoutPageCartItemContainer justifyContent={'right'}>
				Total: {cartItemsStore.cartItemsTotalPrice.toFixed(2)}
			</StyledCheckoutPageCartItemContainer>
		</StyledCheckoutPageContainer>
	);
};
