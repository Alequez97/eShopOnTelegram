import React, { useState } from "react";
import classes from "./Card.module.scss";
import Button from "../Button/Button";
import Product from "../../types/Product";

interface CardProps {
  product: Product
  onAdd: (product: Product) => void;
  onRemove: (product: Product) => void;
}

function Card({ product, onAdd, onRemove }: CardProps) {
  const [productQuantityAddedInCart, setProductQuantityAddedInCart] = useState(0);
  const { productName, image, originalPrice } = product;

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
      <div className={classes.imageContainer}>
        <img src={'https://math-media.byjusfutureschool.com/bfs-math/2022/07/04185628/Asset-1-8-300x300.png'} alt={productName} />
      </div>
      <h4 className={classes.cardTitle}>
        {productName}
        <br />
        <span className={classes.cardPrice}>{originalPrice} €</span>
        <br />
        <i>
          Available: {product.quantityLeft < 20 ? product.quantityLeft : '20+'}
        </i>
      </h4>


      <div className={classes.btnContainer}>
        {productQuantityAddedInCart === 0 && <Button title={"Add"} type={"add"} onClick={handleIncrement} disabled={false} />}

        {productQuantityAddedInCart !== 0 && <Button title={"-"} type={"remove"} onClick={handleDecrement} disabled={false} />}
        <span
          className={`${productQuantityAddedInCart !== 0 ? classes.cardBadge : classes.cardBadgeHidden}`}
        >
          {productQuantityAddedInCart}
        </span>
        {productQuantityAddedInCart !== 0 && <Button title={"+"} type={"add"} onClick={handleIncrement} disabled={false} />}
      </div>
    </div>
  );
}

export default Card;