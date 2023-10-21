import { styled } from 'styled-components';

interface StyledProductAttributeSelectorWrapperProps {
	$isVisible: boolean;
}

export const StyledProductAttributeSelectorWrapper = styled.div<StyledProductAttributeSelectorWrapperProps>`
	display: ${(props) => (props.$isVisible ? 'flex' : 'none')};
	overflow-x: auto;
	gap: 10px;
	padding: 10px;
	max-width: 100%;
`;

interface StyledProductAttributeTextProps {
	$isSelected: boolean;
}

export const StyledProductAttributeOptions = styled.span<StyledProductAttributeTextProps>`
	background-color: var(--button-color);
	color: var(--button-text-color);
	border: 1px solid black;
	border: ${(props) =>
		props.$isSelected ? '2px solid var(--text-color)' : 'none'};
	font-weight: ${(props) => (props.$isSelected ? 'bold' : 'normal')};
	border-radius: 50px;
	padding: 10px;
	margin: 0;
	white-space: nowrap;
	cursor: pointer;
`;

export const StyledProductAttributeTitle = styled.span`
	font-weight: bold;
	margin-right: 5px;
	line-height: 45px;
`;
