import styled from 'styled-components';

export const StyledInputTextContainer = styled.div`
	display: flex;
	flex-direction: column;
	row-gap: 0.5rem;
`;

export const StyledInputTextLabel = styled.label``;

interface StyledInputTextProps {
	$hasError: boolean;
}

export const StyledInputText = styled.input<StyledInputTextProps>`
	color: var(--text-color);
	border-color: ${(props) =>
		props.$hasError ? 'tomato' : 'var(--text-color)'};
	background-color: var(--background-color);
	width: 100%;
	height: 2.5rem;
	border-radius: 0.3rem;
	padding: 0.5rem;
`;

export const StyledErrorLabel = styled.span`
	color: tomato;
`;
