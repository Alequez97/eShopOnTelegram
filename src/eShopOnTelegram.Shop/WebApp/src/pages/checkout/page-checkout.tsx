import { useCartItemsStore } from '../../contexts/cart-items-store.context';
import { useNavigate } from 'react-router-dom';
import { RouteLocation } from '../../enums/route-location.enum';
import {
	StyledCheckoutPageBoldText,
	StyledCheckoutPageCartItemContainer,
	StyledCheckoutPageCartItemInformation,
	StyledCheckoutPageContainer,
	StyledCheckoutPagePriceInformation,
} from './checkout.styled';
import { getPropertiesLabel } from '../../utils/product-attribute.utility';
import { Counter } from '../../components/counter/Counter';
import { observer } from 'mobx-react-lite';
import { useTranslations } from '../../contexts/translations.context';
import { useTelegramBackButton } from '../../hooks/telegram/useTelegramBackButton';
import { useTelegramMainButton } from '../../hooks/telegram/useTelegramMainButton';

export const PageCheckout = observer(() => {
	const navigate = useNavigate();
	const translations = useTranslations();

	const cartItemsStore = useCartItemsStore();
	const notEmptyCartItems = cartItemsStore.cartItemsState.filter(
		(cartItem) => cartItem.quantity > 0,
	);
	useTelegramMainButton(
		notEmptyCartItems.length > 0,
		() => navigate(RouteLocation.SHIPPING_INFO),
		translations.continue.toUpperCase(),
	);

	useTelegramBackButton(() => {
		cartItemsStore.removeEmptyCartItems();
		navigate(RouteLocation.PRODUCTS);
	});

	return (
		<StyledCheckoutPageContainer>
			{cartItemsStore.cartItemsState.map((cartItem) => {
				const { productAttribute } = cartItem;

				return (
					<StyledCheckoutPageCartItemContainer
						key={productAttribute.id}
						$justifyContent={'right'}
						$hasBorder={true}
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
									{translations.currencySymbol}
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
				{translations.totalPrice}:{' '}
				{cartItemsStore.cartItemsTotalPrice.toFixed(2)}{' '}
				{translations.currencySymbol}
			</StyledCheckoutPageCartItemContainer>
		</StyledCheckoutPageContainer>
	);
});
