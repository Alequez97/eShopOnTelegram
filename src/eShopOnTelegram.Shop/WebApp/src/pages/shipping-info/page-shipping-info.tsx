import { useTelegramBackButton } from '../../hooks/telegram/useTelegramBackButton';
import { RouteLocation } from '../../enums/route-location.enum';
import { useNavigate } from 'react-router-dom';

// export const StyledInputTextContainer = styled.div``;
//
// export const StyledInputText = styled.input``;

export const PageShippingInfo = () => {
	const navigate = useNavigate();

	useTelegramBackButton(() => {
		navigate(RouteLocation.CHECKOUT);
	});

	return <div>Shipping</div>;
};
