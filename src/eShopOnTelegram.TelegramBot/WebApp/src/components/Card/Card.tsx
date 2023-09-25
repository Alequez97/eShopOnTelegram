import { useState } from "react";
import classes from "./Card.module.scss";
import { Button } from "../button/button";
import { Product } from "../../types/product";
import {
  StyledCardPrice,
  StyledCardTitle,
  StyledImageContainer,
} from "./card.styled";

interface CardProps {
  product: Product;
  onAdd: (product: Product) => void;
  onRemove: (product: Product) => void;
}

function Card({ product, onAdd, onRemove }: CardProps) {
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
    <div className={classes.card}>
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

      <div className={classes.btnContainer}>
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
        <span
          className={`${
            productQuantityAddedInCart !== 0
              ? classes.cardBadge
              : classes.cardBadgeHidden
          }`}
        >
          {productQuantityAddedInCart}
        </span>
        {productQuantityAddedInCart !== 0 && (
          <Button
            title={"+"}
            type={"add"}
            onClick={handleIncrement}
            disabled={false}
          />
        )}
      </div>
    </div>
  );
}

export default Card;
