import { useState } from "react";
import { CartItem } from "../types/cart-item.type";
import { ProductAttribute } from "../types/product-attribute.type";

export function useCartItemsState() {
    const [cartItems, setCartItems] = useState<CartItem[]>([]);
    
    const addProductAttributeToState = (productAttribute: ProductAttribute) => {
        const existingCartItem = cartItems.find(
            (cartItem) => cartItem.productAttribute.id === productAttribute.id
          );
    
          if (existingCartItem) {
            let newState = cartItems.map((cartItem) => {
              if (cartItem.productAttribute.id === productAttribute.id) {
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
            { productAttribute, quantity: 1 },
          ];
    
          setCartItems(newState)
    }

    const removeProductAttributeFromState = (productAttribute: ProductAttribute) => {
        const existingCartItem = cartItems.find(
            (cartItem) => cartItem.productAttribute.id === productAttribute.id
          );
    
          if (existingCartItem?.quantity === 1) {
            let newState = cartItems.filter(
              (cartItem) => cartItem.productAttribute.id !== productAttribute.id
            );
    
            setCartItems(newState)
            return
          }
    
          let newState = cartItems.map((cartItem) => {
            if (cartItem.productAttribute.id === productAttribute.id) {
              return { ...cartItem, quantity: cartItem.quantity - 1 };
            }
            return cartItem;
          });
    
          setCartItems(newState)
    }

    return { cartItems, addProductAttributeToState, removeProductAttributeFromState }
} 