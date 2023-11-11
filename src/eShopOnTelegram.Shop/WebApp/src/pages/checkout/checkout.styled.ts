import { styled } from 'styled-components';
import { CSSProperties } from 'react';
import { StyledBorderMixin } from '../../mixins/border.mixin.styled';

export const StyledCheckoutPageContainer = styled.div`
	padding: 1vh;
`;

interface StyledCheckoutPageCartItemContainerProps {
	$justifyContent: CSSProperties['justifyContent'];
	hasBorder?: boolean;
}

export const StyledCheckoutPageCartItemContainer = styled.div<StyledCheckoutPageCartItemContainerProps>`
	display: flex;
	align-items: center;
	justify-content: ${(props) => props.$justifyContent};
	margin: 1vh 0;
	padding: 1vh 4vw;
	height: 10vh;

	${(props) => props.hasBorder && StyledBorderMixin};
`;

export const StyledCheckoutPageCartItemInformation = styled.div`
	display: flex;
	flex-direction: column;
	justify-content: space-between;
	height: 7vh;
	margin-right: 3vw;
`;

export const StyledCheckoutPagePriceInformation = styled.div`
	display: flex;
	justify-content: right;
`;

export const StyledCheckoutPageBoldText = styled.span`
	font-weight: bold;
`;
