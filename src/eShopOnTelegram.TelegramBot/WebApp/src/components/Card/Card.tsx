import { useState } from "react";
import { Button } from "../button/button";
import { Product } from "../../types/product";
import {
  StyledButtonContainer,
  StyledCard,
  StyledCardBadge,
  StyledCardInfoWrapper,
  StyledCardPrice,
  StyledImageContainer,
} from "./card.styled";
import { ProductAttribute } from "../../types/productAttribute";
import { ProductAttributeSelector } from "../productAttributeSelector/productAttributeSelector";
import { observer } from "mobx-react-lite";
import { CardStore } from "./card.store";
import outOfStockImage from "../../assets/out_of_stock.jpg";

interface CardProps {
  product: Product;
  onAdd: (productAttribute: ProductAttribute) => void;
  onRemove: (productAttribute: ProductAttribute) => void;
}

export const Card = observer(({ product, onAdd, onRemove }: CardProps) => {
  const [productQuantityAddedInCart, setProductQuantityAddedInCart] =
    useState(0);

  const [cardStore] = useState(new CardStore(product.productAttributes));

  const { name } = product;

  const handleIncrement = () => {
    if (
      productQuantityAddedInCart <
      cardStore.getSelectedProductAttribute.quantityLeft
    ) {
      setProductQuantityAddedInCart(productQuantityAddedInCart + 1);
      onAdd(cardStore.getSelectedProductAttribute);
    }
  };
  const handleDecrement = () => {
    setProductQuantityAddedInCart(productQuantityAddedInCart - 1);
    onRemove(cardStore.getSelectedProductAttribute);
  };

  return (
    <StyledCard>
      <StyledImageContainer>
        {cardStore.getSelectedProductAttribute ? (
          <img src={cardStore.getSelectedProductAttribute.image} alt={name} />
        ) : (
          <img src={outOfStockImage} alt={"Out of stock"} />
        )}
      </StyledImageContainer>
      <StyledCardInfoWrapper>
        {cardStore.getSelectedProductAttribute && name}
        <br />
        <StyledCardPrice>
          {cardStore.getSelectedProductAttribute && (
            <>{cardStore.getSelectedProductAttribute.originalPrice} €</>
          )}
        </StyledCardPrice>
        <br />
        {cardStore.getSelectedProductAttribute ? (
          <i>
            Available:{" "}
            {cardStore.getSelectedProductAttribute.quantityLeft < 20
              ? cardStore.getSelectedProductAttribute.quantityLeft
              : "20+"}
          </i>
        ) : (
          <i>Available: 0</i>
        )}
        <ProductAttributeSelector
          productAttributeName="Color"
          productAttributeValues={[
            ...new Set(
              product.productAttributes
                .map((productAttribute) => productAttribute.color)
                .filter((color) => color !== undefined) as string[]
            ),
          ]}
          onSelection={(selectedColor: string | null) => {
            if (selectedColor) {
              cardStore.setSelectedColor(selectedColor);
            }
          }}
        />
        <ProductAttributeSelector
          productAttributeName="Size"
          productAttributeValues={[
            ...new Set(
              product.productAttributes
                .map((productAttribute) => productAttribute.size)
                .filter((size) => size !== undefined) as string[]
            ),
          ]}
          onSelection={(selectedSize: string | null) => {
            if (selectedSize) {
              cardStore.setSelectedSize(selectedSize);
            }
          }}
        />
      </StyledCardInfoWrapper>

      <StyledButtonContainer>
        {productQuantityAddedInCart === 0 && (
          <Button
            title={"Add"}
            type={"add"}
            onClick={handleIncrement}
            disabled={!cardStore.selectionStateIsValid}
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
        <StyledCardBadge $isVisible={productQuantityAddedInCart !== 0}>
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
});
