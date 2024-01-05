import styled from 'styled-components';
import { StyledBorderMixin } from '../../mixins/border.mixin.styled';

export const StyledCard = styled.div`
	display: flex;
	flex-direction: column;
	justify-content: space-between;
	width: 150px;
	padding: 0.5rem 0.3rem;
	margin: 13px 10px;
	position: relative;
	${StyledBorderMixin}
`;

export const StyledCardInfoWrapper = styled.div`
    margin: 6px 0 12px;
	font-weight: 400;
	text-align: center;
`;

export const StyledImageContainer = styled.div`
    display: flex;
    align-items: center;
    height: 100%;

	img {
		width: 100%;
		object-fit: cover;
	}
`;

export const StyledCardPrice = styled.span`
	text-align: center;
	display: inline-block;
	font-weight: bold;
`;
