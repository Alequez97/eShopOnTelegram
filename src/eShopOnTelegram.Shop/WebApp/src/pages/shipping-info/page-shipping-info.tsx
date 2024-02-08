import { useTelegramBackButton } from '../../hooks/telegram/useTelegramBackButton';
import { RouteLocation } from '../../enums/route-location.enum';
import { useNavigate } from 'react-router-dom';
import { InputText } from '../../components/input-text/input-text';
import { useForm } from 'react-hook-form';
import { useTelegramMainButton } from '../../hooks/telegram/useTelegramMainButton';
import { useBackendTranslations } from '../../contexts/translations.context';
import { StyledShippingInfoFormContainer } from './page-shipping-info.styled';
import { getOrderCreationRequestBodyAsJsonString } from '../../utils/cart-items.utility';
import { useCartItemsStore } from '../../contexts/cart-items-store.context';
import { useTelegramWebApp } from '../../hooks/telegram/useTelegramWebApp';
import { useTranslation } from 'react-i18next';

export const PageShippingInfo = () => {
	const telegramWebApp = useTelegramWebApp();
	const navigate = useNavigate();
	const backendTranslations = useBackendTranslations();
	const cartItemsStore = useCartItemsStore();
	const {
		register,
		formState: { errors, isValid: isFormValid },
		control: { _formValues },
		handleSubmit: handleFormSubmit,
	} = useForm();

	useTelegramBackButton(() => {
		navigate(RouteLocation.CHECKOUT);
	});

	useTelegramMainButton(
		true,
		() => {
			// Calling react-hook-form function to trigger validation and show validation errors
			handleFormSubmit((fieldValues) => {
				console.log(fieldValues);
			})();

			if (!isFormValid) {
				return;
			}

			const notEmptyCartItems = cartItemsStore.cartItemsState.filter(
				(cartItem) => cartItem.quantity > 0,
			);

			const json = getOrderCreationRequestBodyAsJsonString(
				notEmptyCartItems,
				_formValues,
			);
			telegramWebApp.sendData(json);
		},
		backendTranslations.proceedToPayment.toUpperCase(),
	);

	const { t } = useTranslation();

	return (
		<div>
			<StyledShippingInfoFormContainer>
				<h2>{t('delivery_information')}</h2>
				<InputText
					register={register}
					name="country"
					label={t('country')}
					isRequired={true}
					errorLabel={errors?.country?.message as string}
				/>
				<InputText
					register={register}
					name="city"
					label={t('city')}
					isRequired={true}
					errorLabel={errors?.city?.message as string}
				/>
				<InputText
					register={register}
					name="streetLine1"
					label={t('street_line_1')}
					isRequired={true}
					errorLabel={errors?.streetLine1?.message as string}
				/>
				<InputText
					register={register}
					name="streetLine2"
					label={t('street_line_2')}
					isRequired={false}
				/>
				<InputText
					register={register}
					name="postCode"
					label={t('post_code')}
					isRequired={true}
					errorLabel={errors?.postCode?.message as string}
				/>
			</StyledShippingInfoFormContainer>
		</div>
	);
};
