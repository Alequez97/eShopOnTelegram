import React, { useState } from "react";
import "./Card.scss";
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
    <div className="card">
      <div className="image__container">
        <img src={image} alt={productName} />
      </div>
      <h4 className="card__title">
        {productName}
        <br />
        <span className="card__price">{originalPrice} €</span>
        <br />
        <i>
          Available: {product.quantityLeft < 20 ? product.quantityLeft : '20+'}
        </i>
      </h4>


      <div className="btn-container">
        {productQuantityAddedInCart === 0 && <Button title={"Add"} type={"add"} onClick={handleIncrement} disabled={false} />}

        {productQuantityAddedInCart !== 0 && <Button title={"-"} type={"remove"} onClick={handleDecrement} disabled={false} />}
        <span
          className={`${productQuantityAddedInCart !== 0 ? "card__badge" : "card__badge--hidden"}`}
        >
          {productQuantityAddedInCart}
        </span>
        {productQuantityAddedInCart !== 0 && <Button title={"+"} type={"add"} onClick={handleIncrement} disabled={false} />}
      </div>
    </div>
  );
}

export default Card;