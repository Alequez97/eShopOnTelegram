import { useState } from "react";
import { Button } from "../button/button";
import { Product } from "../../types/product";
import {
  StyledButtonContainer,
  StyledCard,
  StyledCardBadge,
  StyledCardInfoWrapper,
  StyledCardPrice,
  StyledImageContainer,
} from "./card.styled";
import { ProductAttribute } from "../../types/productAttribute";
import { ProductAttributeSelector } from "../productAttributeSelector/productAttributeSelector";

interface CardProps {
  product: Product;
  onAdd: (productAttribute: ProductAttribute) => void;
  onRemove: (productAttribute: ProductAttribute) => void;
}

export const Card = ({ product, onAdd, onRemove }: CardProps) => {
  const [productQuantityAddedInCart, setProductQuantityAddedInCart] =
    useState(0);
  const [selectedProductAttribute, setSelectedProductAttribute] = useState(
    product.productAttributes[0]
  );

  const { name } = product;

  const handleIncrement = () => {
    if (productQuantityAddedInCart < selectedProductAttribute.quantityLeft) {
      setProductQuantityAddedInCart(productQuantityAddedInCart + 1);
      onAdd(selectedProductAttribute);
    }
  };
  const handleDecrement = () => {
    setProductQuantityAddedInCart(productQuantityAddedInCart - 1);
    onRemove(selectedProductAttribute);
  };

  return (
    <StyledCard>
      <StyledImageContainer>
        <img src={selectedProductAttribute.image} alt={name} />
      </StyledImageContainer>
      <StyledCardInfoWrapper>
        {name}
        <br />
        <StyledCardPrice>
          {selectedProductAttribute.originalPrice} €
        </StyledCardPrice>
        <br />
        <i>
          Available:{" "}
          {selectedProductAttribute.quantityLeft < 20
            ? selectedProductAttribute.quantityLeft
            : "20+"}
        </i>
        <ProductAttributeSelector
          productAttributeName="Color"
          productAttributeValues={
            product.productAttributes
              .map((productAttribute) => productAttribute.color)
              .filter((color) => color !== undefined) as string[]
          }
        />
        <ProductAttributeSelector
          productAttributeName="Size"
          productAttributeValues={
            product.productAttributes
              .map((productAttribute) => productAttribute.size)
              .filter((size) => size !== undefined) as string[]
          }
        />
      </StyledCardInfoWrapper>

      <StyledButtonContainer>
        {productQuantityAddedInCart === 0 && (
          <Button
            title={"Add"}
            type={"add"}
            onClick={handleIncrement}
            disabled={false}
          />
        )}
        {productQuantityAddedInCart !== 0 && (
          <Button
            title={"-"}
            type={"remove"}
            onClick={handleDecrement}
            disabled={false}
          />
        )}
        <StyledCardBadge $isVisible={productQuantityAddedInCart !== 0}>
          {productQuantityAddedInCart}
        </StyledCardBadge>
        {productQuantityAddedInCart !== 0 && (
          <Button
            title={"+"}
            type={"add"}
            onClick={handleIncrement}
            disabled={false}
          />
        )}
      </StyledButtonContainer>
    </StyledCard>
  );
};
