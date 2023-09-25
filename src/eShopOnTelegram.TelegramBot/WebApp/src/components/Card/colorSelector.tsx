import React, { useState } from "react";
import styled from "styled-components";

const ColorSelectorWrapper = styled.div`
  display: flex;
  overflow-x: auto;
  gap: 10px;
  padding: 10px;
  max-width: 100%; /* Ensure it takes full available width */
`;

const ColorText = styled.span<{ isSelected: boolean }>`
  background-color: ${(props) => (props.isSelected ? "gray" : "inherit")};
  color: black;
  border-radius: 50px;
  padding: 10px;
  margin: 0;
  white-space: nowrap; /* Prevent text from breaking to a new line */
  cursor: pointer;
`;

const Title = styled.span`
  font-weight: bold;
  margin-right: 5px;
  line-height: 45px; /* Vertically center the title */
`;

interface ColorSelectorProps {
  colors: string[];
}

export const ColorSelector = ({ colors }: ColorSelectorProps) => {
  const [selectedColor, setSelectedColor] = useState<string | null>(null);

  const handleColorClick = (color: string) => {
    setSelectedColor(color);
  };

  return (
    <div>
      <ColorSelectorWrapper>
        <Title>Color:</Title>
        {colors?.map((color, index) => (
          <ColorText
            key={index}
            isSelected={selectedColor === color}
            onClick={() => handleColorClick(color)}
          >
            {color}
          </ColorText>
        ))}
      </ColorSelectorWrapper>
    </div>
  );
};
