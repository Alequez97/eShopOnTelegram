import { useState } from "react";
import CartItem from "../types/cart-item";
import Product from "../types/product";

export function useCartItems() {
    const [cartItems, setCartItems] = useState<CartItem[]>([]);
    
    const addProductToState = (product: Product) => {
        const existingCartItem = cartItems.find(
            (cartItem) => cartItem.product.id === product.id
          );
    
          if (existingCartItem) {
            let newState = cartItems.map((cartItem) => {
              if (cartItem.product.id === product.id) {
                return {
                  ...existingCartItem,
                  quantity: existingCartItem.quantity + 1,
                };
              }
              return cartItem;
            });
    
            setCartItems(newState)
            return
          }
    
          let newState = [
            ...cartItems,
            { product: product, quantity: 1 },
          ];
    
          setCartItems(newState)
    }

    const removeProductFromState = (product: Product) => {
        const existingCartItem = cartItems.find(
            (cartItem) => cartItem.product.id === product.id
          );
    
          if (existingCartItem?.quantity === 1) {
            let newState = cartItems.filter(
              (cartItem) => cartItem.product.id !== product.id
            );
    
            setCartItems(newState)
            return
          }
    
          let newState = cartItems.map((cartItem) => {
            if (cartItem.product.id === product.id) {
              return { ...cartItem, quantity: cartItem.quantity - 1 };
            }
            return cartItem;
          });
    
          setCartItems(newState)
    }

    return { cartItems, addProductToState, removeProductFromState }
} 