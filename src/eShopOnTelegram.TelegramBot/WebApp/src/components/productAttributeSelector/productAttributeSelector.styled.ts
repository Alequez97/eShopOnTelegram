import { styled } from "styled-components";

interface StyledProductAttributeSelectorWrapperProps {
  $isVisible: boolean;
}

export const StyledProductAttributeSelectorWrapper = styled.div<StyledProductAttributeSelectorWrapperProps>`
  display: ${(props) => (props.$isVisible ? "flex" : "none")};
  overflow-x: auto;
  gap: 10px;
  padding: 10px;
  max-width: 100%;
`;

interface StyledProductAttributeTextProps {
  $isSelected: boolean;
}

export const StyledProductAttributeOptions = styled.span<StyledProductAttributeTextProps>`
  background-color: ${(props) => (props.$isSelected ? "gray" : "inherit")};
  border: 1px solid black;
  color: black;
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
