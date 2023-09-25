import React, { MouseEventHandler } from "react";
import classes from "./Cart.module.scss";
import Button from "../button/button";
import ICartItem from "../../types/cart-item";

interface CartProps {
    cartItems: ICartItem[]
    onCheckout: MouseEventHandler<HTMLElement>
}

function Cart({ cartItems, onCheckout }: CartProps) {
  return (
    <div className={classes.cartContainer}>
      <Button
        title={"Checkout"}
        type={"checkout"}
        disabled={cartItems.length === 0}
        onClick={onCheckout}
      />
    </div>
  );
}

export default Cart;