import { useState } from "react";
import { Button } from "../button/button";
import { Product } from "../../types/product";
import {
  StyledButtonContainer,
  StyledCard,
  StyledCardBadge,
  StyledCardPrice,
  StyledCardTitle,
  StyledImageContainer,
} from "./card.styled";

interface CardProps {
  product: Product;
  onAdd: (product: Product) => void;
  onRemove: (product: Product) => void;
}

export const Card = ({ product, onAdd, onRemove }: CardProps) => {
  const [productQuantityAddedInCart, setProductQuantityAddedInCart] =
    useState(0);
  const { name, image, originalPrice } = product;

  const handleIncrement = () => {
    if (productQuantityAddedInCart < product.quantityLeft) {
      setProductQuantityAddedInCart(productQuantityAddedInCart + 1);
      onAdd(product);
    }
  };
  const handleDecrement = () => {
    setProductQuantityAddedInCart(productQuantityAddedInCart - 1);
    onRemove(product);
  };

  return (
    <StyledCard>
      <StyledImageContainer>
        <img src={image} alt={name} />
      </StyledImageContainer>
      <StyledCardTitle>
        {name}
        <br />
        <StyledCardPrice>{originalPrice} €</StyledCardPrice>
        <br />
        <i>
          Available: {product.quantityLeft < 20 ? product.quantityLeft : "20+"}
        </i>
      </StyledCardTitle>

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
        <StyledCardBadge visible={productQuantityAddedInCart !== 0}>
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
}