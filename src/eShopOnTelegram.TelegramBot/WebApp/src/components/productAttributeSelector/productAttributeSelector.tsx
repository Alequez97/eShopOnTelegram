import { useEffect, useState } from "react";
import {
  StyledProductAttributeOptions,
  StyledProductAttributeSelectorWrapper,
  StyledProductAttributeTitle,
} from "./productAttributeSelector.styled";

interface ProductAttributeSelectorProps {
  productAttributeName: string;
  productAttributeValues: string[] | null;
  onSelection: (productAttributeValue: string | null) => void;
}

export const ProductAttributeSelector = ({
  productAttributeName,
  productAttributeValues,
  onSelection,
}: ProductAttributeSelectorProps) => {
  const [selectedProductAttribute, setSelectedProductAttribute] = useState<
    string | null
  >(
    productAttributeValues === null ? null : productAttributeValues![0] ?? null
  );

  if (selectedProductAttribute) {
    onSelection(selectedProductAttribute);
  }

  const onProductAttributeSelect = (productAttribute: string | null) => {
    setSelectedProductAttribute(productAttribute);
    onSelection(productAttribute);
  };

  useEffect(() => {
    if (productAttributeValues?.length === 1) {
      onProductAttributeSelect(productAttributeValues[0]);
    }
  }, []);

  return (
    <div>
      <StyledProductAttributeSelectorWrapper
        $isVisible={
          productAttributeValues !== null && productAttributeValues.length > 0
        }
      >
        <StyledProductAttributeTitle>
          {productAttributeName}:
        </StyledProductAttributeTitle>
        {productAttributeValues?.map((productAttributeValue, index) => (
          <StyledProductAttributeOptions
            key={index}
            $isSelected={selectedProductAttribute === productAttributeValue}
            onClick={() => onProductAttributeSelect(productAttributeValue)}
          >
            {productAttributeValue}
          </StyledProductAttributeOptions>
        ))}
      </StyledProductAttributeSelectorWrapper>
    </div>
  );
};
