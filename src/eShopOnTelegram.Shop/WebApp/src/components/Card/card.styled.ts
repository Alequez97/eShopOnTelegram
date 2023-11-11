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

export const StyledCardInfoWrapper = styled.h4`
	font-weight: 400;
	text-align: center;
`;

export const StyledImageContainer = styled.div`
	width: 100px;
	height: 100px;
	margin: 0 auto;

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
