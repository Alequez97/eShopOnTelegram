import styled, { keyframes } from 'styled-components';

const ringLoaderAnimation = keyframes`
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }`;

export const StyledRingLoader = styled.div`
	position: absolute;
	top: 50%;
	left: 50%;
	transform: translate(-50%, -50%);
	width: 150px;
	height: 150px;
	background: transparent;
	border: 3px solid #3c3c3c;
	border-radius: 50%;
	text-align: center;
	line-height: 150px;
	font-family: sans-serif;
	font-size: 20px;
	color: var(--text-color);
	letter-spacing: 4px;
	text-transform: uppercase;
	text-shadow: 0 0 10px var(--text-color);
	box-shadow: 0 0 20px rgba(0, 0, 0, 0.5);

	&::before {
		content: '';
		position: absolute;
		top: -3px;
		left: -3px;
		width: 100%;
		height: 100%;
		border: 3px solid transparent;
		border-top: 3px solid var(--text-color);
		border-right: 3px solid var(--text-color);
		border-radius: 50%;
		animation: ${ringLoaderAnimation} 2s linear infinite;
	}
`;

const ringLoaderSpanAnimation = keyframes`
  0% {
    transform: rotate(45deg);
  }
  100% {
    transform: rotate(405deg);
  }`;

export const StyledRingLoaderSpan = styled.div`
	display: block;
	position: absolute;
	top: calc(50% - 2px);
	left: 50%;
	width: 50%;
	height: 4px;
	background: transparent;
	transform-origin: left;
	animation: ${ringLoaderSpanAnimation} 2s linear infinite;

	&::before {
		content: '';
		position: absolute;
		width: 16px;
		height: 16px;
		border-radius: 50%;
		background: var(--text-color);
		top: -6px;
		right: -8px;
		box-shadow: 0 0 20px var(--text-color);
	}
`;
