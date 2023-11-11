import styled from 'styled-components';
import { MouseEventHandler } from 'react';

interface CounterButtonProps {
	type: 'add' | 'remove';
	title: string;
	disabled: boolean;
	onClick: MouseEventHandler<HTMLElement>;
}

// eslint-disable-next-line @typescript-eslint/no-explicit-any
const buttonStyles: Record<CounterButtonProps['type'], any> = {
	add: {
		normal: 'rgb(75, 226, 75)',
		hover: '#ad9a1c',
		active: '#0b8325',
	},
	remove: {
		normal: 'tomato',
		hover: 'rgb(209, 83, 61)',
		active: 'rgb(189, 83, 61)',
	},
};

const StyledCounterButton = styled.button<CounterButtonProps>`
	color: black;
	padding: 0.6rem 0.8rem;
	font-size: 1.2rem;
	text-align: center;
	border: 0;
	outline: none;
	border-radius: 10px;
	width: 120px;
	margin-left: 10px;
	box-shadow: 1px -3px 12px -5px var(--text-color);
	background-color: ${(props) => buttonStyles[props.type].normal};

	&:hover {
		background-color: ${(props) => buttonStyles[props.type].hover};
	}

	&:active {
		background-color: ${(props) => buttonStyles[props.type].active};
		box-shadow: none;
	}

	&:disabled {
		background-color: #ccc;
		color: #666;
		cursor: not-allowed;
	}
`;

export const CounterButton = (props: CounterButtonProps) => {
	return <StyledCounterButton {...props}>{props.title}</StyledCounterButton>;
};

export const StyledCounterContainer = styled.div`
	display: flex;
	align-items: center;
	justify-content: center;

	button {
		margin: 0 auto;
	}
`;

interface StyledCounterValueBadgeProps {
	$isVisible: boolean;
}

export const StyledCounterValueBadge = styled.div<StyledCounterValueBadgeProps>`
	display: ${(props) => (props.$isVisible ? 'flex' : 'none')};
	justify-content: center;
	align-items: center;
	width: 90px;
	color: var(--text-color);
	font-weight: bold;
	text-align: center;
	font-size: 20px;
`;
