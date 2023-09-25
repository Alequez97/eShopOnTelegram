import { ChangeEvent, useCallback, useEffect, useState } from "react";
import { Card } from "../../components/card/card";
import { Loader } from "../../components/loader/loader";
import { Error } from "../../components/error/error";
import { getCartItemsAsJsonString } from "../../utilities/cartItems";
import { useCartItems } from "../../hooks/cartItems";
import { useTelegramWebApp } from "../../hooks/telegram";
import { Product } from "../../types/product";
import { useProducts } from "../../hooks/products";
import {
  StyledCardsContainer,
  StyledMissingProductsMessageWrapper,
  StyledProductCategoriesSelect,
  StyledProductCategoriesWrapper,
} from "../styled/products.styled";

export const Products = () => {
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
  }, [cartItems]);

  const sendDataToTelegram = useCallback(() => {
    const json = getCartItemsAsJsonString(cartItems);
    telegramWebApp.sendData(json);
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
      <h2 style={{ textAlign: "center" }}>eShopOnTelegram</h2>

      {products.length === 0 && (
        <StyledMissingProductsMessageWrapper>
          <span>No available products at this moment</span>
        </StyledMissingProductsMessageWrapper>
      )}

      {products.length !== 0 && (
        <StyledProductCategoriesWrapper>
          <StyledProductCategoriesSelect
            name="product-categories"
            defaultValue={DEFAULT_SELECTOR_VALUE}
            onChange={selectOnChangeHandler}
          >
            <option value={DEFAULT_SELECTOR_VALUE} key={DEFAULT_SELECTOR_VALUE}>
              {DEFAULT_SELECTOR_VALUE}
            </option>
            {productCategories?.map((category) => (
              <option value={category} key={category}>
                {category}
              </option>
            ))}
          </StyledProductCategoriesSelect>
        </StyledProductCategoriesWrapper>
      )}

      <StyledCardsContainer>
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
      </StyledCardsContainer>
    </>
  );
};
