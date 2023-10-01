import { useEffect, useState } from "react";
import {
  StyledProductAttributeOptions,
  StyledProductAttributeSelectorWrapper,
  StyledProductAttributeTitle,
} from "./productAttributeSelector.styled";
import { observer } from "mobx-react-lite";

interface ProductAttributeSelectorProps {
  productAttributeName: string;
  productAttributeValues: string[] | null;
  selectedProductAttribute: string | null;
  onSelection: (productAttributeValue: string) => void;
}

export const ProductAttributeSelector = observer(
  ({
    productAttributeName,
    productAttributeValues,
    selectedProductAttribute,
    onSelection,
  }: ProductAttributeSelectorProps) => {
    return (
      <div>
        <StyledProductAttributeSelectorWrapper
          $isVisible={
            productAttributeValues !== null && productAttributeValues.length > 0
          }
        >
          {/* <StyledProductAttributeTitle>
            {productAttributeName}:
          </StyledProductAttributeTitle> */}
          {productAttributeValues?.map((productAttributeValue, index) => (
            <StyledProductAttributeOptions
              key={index}
              $isSelected={selectedProductAttribute === productAttributeValue}
              onClick={() => onSelection(productAttributeValue)}
            >
              {productAttributeValue}
            </StyledProductAttributeOptions>
          ))}
        </StyledProductAttributeSelectorWrapper>
      </div>
    );
  }
);
