import styled from 'styled-components';

export const StyledProductCategoriesSelect = styled.select`
	background-color: var(--background-color);
	color: var(--primary-color);
	border-color: var(--primary-color);
	border-radius: 10px;
	font-size: 3vh;
	text-align: center;
	width: 70vw;
	height: 6vh;
	margin: 2vh 0;

	&:focus {
		border-color: var(--primary-color);
	}
`;

export const StyledCardsContainer = styled.div`
	display: flex;
	flex-wrap: wrap;
	justify-content: center;
	padding-bottom: 100px;
`;

export const StyledProductCategoriesWrapper = styled.div`
	display: flex;
	justify-content: center;
`;

export const StyledMissingProductsMessageWrapper = styled.div`
	text-align: center;
	font-size: 18px;
	margin-top: 50%;
`;
