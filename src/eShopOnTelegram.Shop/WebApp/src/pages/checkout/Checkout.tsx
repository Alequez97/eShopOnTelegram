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
	StyledCheckoutPageCartItemInformation,
	StyledCheckoutPagePriceInformation,
} from './checkout.styled';
import { getPropertiesLabel } from '../../utils/product-attribute.utility';
import { Counter } from '../../components/counter/Counter';
import { observer } from 'mobx-react-lite';
import { useTranslations } from '../../contexts/translations.context';

export const Checkout = observer(() => {
	const navigate = useNavigate();
	const telegramWebApp = useTelegramWebApp();
	const translations = useTranslations();

	telegramWebApp.MainButton.setText(
		translations.proceedToPayment.toUpperCase(),
	);

	const cartItemsStore = useCartItemsStore();

	useEffect(() => {
		telegramWebApp.BackButton?.show();

		const navigateToProducts = () => {
			cartItemsStore.removeEmptyCartItems();
			navigate(RouteLocation.PRODUCTS);
		};

		telegramWebApp.BackButton?.onClick(navigateToProducts);

		return () => {
			telegramWebApp.BackButton?.hide();
			telegramWebApp.BackButton?.offClick(navigateToProducts);
		};
	}, []);

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
						$justifyContent={'right'}
						hasBorder={true}
					>
						<StyledCheckoutPageCartItemInformation>
							<div>
								<StyledCheckoutPageBoldText>
									{productAttribute.productName}
								</StyledCheckoutPageBoldText>{' '}
								<span>
									{getPropertiesLabel(productAttribute)}{' '}
								</span>
							</div>
							<StyledCheckoutPagePriceInformation>
								<span>
									{productAttribute.priceWithDiscount
										? productAttribute.priceWithDiscount
										: productAttribute.originalPrice}{' '}
									€
								</span>
							</StyledCheckoutPagePriceInformation>
						</StyledCheckoutPageCartItemInformation>
						<div style={{ width: '30vw' }}>
							<Counter
								value={cartItem.quantity}
								showAddButton={false}
								showPlusButton={true}
								showMinusButton={true}
								onAdd={() => {
									cartItemsStore.addProductAttribute(
										productAttribute,
									);
								}}
								onRemove={() => {
									cartItemsStore.removeProductAttribute(
										productAttribute,
									);
								}}
							/>
						</div>
					</StyledCheckoutPageCartItemContainer>
				);
			})}
			<StyledCheckoutPageCartItemContainer $justifyContent={'right'}>
				Total: {cartItemsStore.cartItemsTotalPrice.toFixed(2)}
			</StyledCheckoutPageCartItemContainer>
		</StyledCheckoutPageContainer>
	);
});
