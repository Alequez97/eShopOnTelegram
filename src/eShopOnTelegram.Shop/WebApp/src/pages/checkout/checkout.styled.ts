import { css, styled } from 'styled-components';
import { CSSProperties } from 'react';

export const StyledCheckoutPageContainer = styled.div`
	padding: 1vh;
`;

interface StyledCheckoutPageCartItemContainerProps {
	justifyContent: CSSProperties['justifyContent'];
	hasBorder?: boolean;
}

export const StyledCheckoutPageCartItemContainer = styled.div<StyledCheckoutPageCartItemContainerProps>`
	display: flex;
	align-items: center;
	justify-content: ${(props) => props.justifyContent};
	margin: 1vh 0;
	padding: 1vh 4vw;
	height: 10vh;

	${(props) =>
		props.hasBorder &&
		css`
			border-radius: 5px;
			border: 2px solid black;
		`}
`;

export const StyledCheckoutPageBoldText = styled.span`
	font-weight: bold;
`;
