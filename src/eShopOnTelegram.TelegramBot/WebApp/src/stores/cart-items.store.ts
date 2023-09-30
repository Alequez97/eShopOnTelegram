import { makeAutoObservable } from "mobx";
import { CartItem } from "../types/cart-item";

class CartItemsStore {
  private cartItems: CartItem[];

  constructor() {
    this.cartItems = [];
    makeAutoObservable(this);
  }

  get getCartItems() {
    return this.cartItems;
  }

  updateCartItems(updatedCartItems: CartItem[]) {
    if (!updatedCartItems) {
      return;
    }

    console.log("updateCartItems");

    this.cartItems = [
      {
        productAttribute: {
          id: 1,
          image: "",
          originalPrice: 1,
          quantityLeft: 10,
        },
        quantity: 1,
      },
    ];

    // const newCartItems: CartItem[] = [];

    // for (const updatedCartItem of updatedCartItems) {
    //   const existingCartItemIndex = this.cartItems.findIndex(
    //     (existingCartItems) =>
    //       existingCartItems.productAttribute.id ===
    //       updatedCartItem.productAttribute.id
    //   );

    //   if (existingCartItemIndex !== -1) {
    //     newCartItems.push(updatedCartItem);
    //   } else {
    //     newCartItems.push({
    //       ...this.cartItems[existingCartItemIndex],
    //       quantity: updatedCartItem.quantity,
    //     });
    //   }
    // }

    // const notUpdatedCartItems: CartItem[] = [];
    // for (const existingCartItem of this.cartItems) {
    //   const updatedCartItemIndex = updatedCartItems.findIndex(
    //     (updatedCartItem) =>
    //       updatedCartItem.productAttribute.id ===
    //       existingCartItem.productAttribute.id
    //   );

    //   if (updatedCartItemIndex === -1) {
    //     notUpdatedCartItems.push(existingCartItem);
    //   }
    // }

    // this.cartItems = [...notUpdatedCartItems, ...newCartItems];
  }
}

const cartItemsStore = new CartItemsStore();
export default cartItemsStore;
