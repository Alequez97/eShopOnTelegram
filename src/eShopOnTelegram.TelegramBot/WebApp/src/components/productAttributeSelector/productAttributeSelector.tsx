import { useState } from "react";
import {
  StyledProductAttributeOptions,
  StyledProductAttributeSelectorWrapper,
  StyledProductAttributeTitle,
} from "./productAttributeSelector.styled";
import { ProductAttribute } from "../../types/productAttribute";

interface ProductAttributeSelectorProps {
  productAttributeName: string;
  productAttributeValues: string[];
}

export const ProductAttributeSelector = ({
  productAttributeName,
  productAttributeValues,
}: ProductAttributeSelectorProps) => {
  const [selectedProductAttribute, setSelectedProductAttribute] = useState<
    string | null
  >(null);

  return (
    <div>
      <StyledProductAttributeSelectorWrapper
        $isVisible={productAttributeValues.length > 0}
      >
        <StyledProductAttributeTitle>
          {productAttributeName}:
        </StyledProductAttributeTitle>
        {productAttributeValues.map((productAttributeValue, index) => (
          <StyledProductAttributeOptions
            key={index}
            $isSelected={selectedProductAttribute === productAttributeValue}
            onClick={() => setSelectedProductAttribute(productAttributeValue)}
          >
            {productAttributeValue}
          </StyledProductAttributeOptions>
        ))}
      </StyledProductAttributeSelectorWrapper>
    </div>
  );
};
