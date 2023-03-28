import React, { ChangeEvent, useCallback, useEffect, useState } from "react";
import classes from "./Products.module.scss";
import Card from "../../components/Card/Card";
import Loader from "../../components/Loader/Loader";
import Error from "../../components/Error/Error";
import { getCartItemsAsJsonString } from "../../utilities/cartItems";
import { useCartItems } from "../../hooks/cartItems";
import { useTelegramWebApp } from "../../hooks/telegram";
import Product from "../../types/Product";
import { useProducts } from "../../hooks/products";

export default function Products() {
  const { telegramWebApp } = useTelegramWebApp();

  useEffect(() => {
    telegramWebApp.expand();
    // eslint-disable-next-line
  }, []);

  const { cartItems, addProductToState, removeProductFromState } =
    useCartItems();
  const { products, productCategories, error, loading } = useProducts();
  const [filteredProducts, setFilteredProducts] = useState<
    Product[] | undefined
  >(undefined);

  useEffect(() => {
    if (cartItems.length === 0) {
      telegramWebApp.MainButton.hide();
    } else {
      telegramWebApp.MainButton.show();
    }
    // eslint-disable-next-line
  }, [cartItems]);

  const sendDataToTelegram = useCallback(() => {
    const json = getCartItemsAsJsonString(cartItems);
    telegramWebApp.sendData(json);
    // eslint-disable-next-line
  }, [cartItems]);

  useEffect(() => {
    telegramWebApp.onEvent("mainButtonClicked", sendDataToTelegram);
    return () => {
      telegramWebApp.offEvent("mainButtonClicked", sendDataToTelegram);
    };
    // eslint-disable-next-line
  }, [sendDataToTelegram]);

  const onAdd = (product: Product) => {
    addProductToState(product);
  };

  const onRemove = (product: Product) => {
    removeProductFromState(product);
  };

  if (loading) {
    return <Loader />;
  }

  if (error) {
    return <Error />;
  }

  const DEFAULT_SELECTOR_VALUE = "All categories";

  const selectOnChangeHandler = (event: ChangeEvent<HTMLSelectElement>) => {
    const selectedOption = event.target.value;
    if (selectedOption === DEFAULT_SELECTOR_VALUE) {
      setFilteredProducts(undefined);
      return;
    }

    setFilteredProducts(
      products.filter(
        (product) => product.productCategoryName === selectedOption
      )
    );
  };

  return (
    <>
      <h2 className={classes.heading}>eShopOnTelegram</h2>

      {products.length === 0 && (
        <span>No available products at this moment</span>
      )}

      {products.length !== 0 && (
        <div className={classes.productCategoriesWrapper}>
          <select
            name="product-categories"
            className={classes.productCategoriesSelect}
            defaultValue={DEFAULT_SELECTOR_VALUE}
            onChange={selectOnChangeHandler}
          >
            <option value={DEFAULT_SELECTOR_VALUE}>
              {DEFAULT_SELECTOR_VALUE}
            </option>
            {productCategories?.map((category) => (
              <option value={category}>{category}</option>
            ))}
          </select>
        </div>
      )}

      <div className={classes.cardsContainer}>
        {filteredProducts === undefined &&
          products.map((product) => (
            <Card
              product={product}
              key={product.id}
              onAdd={onAdd}
              onRemove={onRemove}
            />
          ))}
        {filteredProducts !== undefined &&
          filteredProducts.map((product) => (
            <Card
              product={product}
              key={product.id}
              onAdd={onAdd}
              onRemove={onRemove}
            />
          ))}
      </div>
    </>
  );
}
