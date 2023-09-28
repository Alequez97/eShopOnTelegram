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

export const Card = observer(({ product }: CardProps) => {
  const [cardStore] = useState(new CardStore(product.productAttributes));
  const { name } = product;

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
        {name}
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
          productAttributeValues={cardStore.getAvailableColors}
          selectedProductAttribute={cardStore.getSelectedColor}
          onSelection={(color: string) => cardStore.setSelectedColor(color)}
        />
        <ProductAttributeSelector
          productAttributeName="Size"
          productAttributeValues={cardStore.getAvailableSizes}
          selectedProductAttribute={cardStore.getSelectedSize}
          onSelection={(color: string) => cardStore.setSelectedSize(color)}
        />
      </StyledCardInfoWrapper>

      <StyledButtonContainer>
        {cardStore.getSelectedProductAttributeQuantity === 0 && (
          <Button
            title={"Add"}
            type={"add"}
            onClick={() => cardStore.increaseSelectedProductAttributeQuantity()}
            disabled={!cardStore.selectionStateIsValid}
          />
        )}
        {cardStore.getSelectedProductAttributeQuantity !== 0 && (
          <Button
            title={"-"}
            type={"remove"}
            onClick={() => cardStore.decreaseSelectedProductAttributeQuantity()}
            disabled={false}
          />
        )}
        <StyledCardBadge
          $isVisible={cardStore.getSelectedProductAttributeQuantity !== 0}
        >
          {cardStore.getSelectedProductAttributeQuantity}
        </StyledCardBadge>
        {cardStore.getSelectedProductAttributeQuantity !== 0 && (
          <Button
            title={"+"}
            type={"add"}
            onClick={() => cardStore.increaseSelectedProductAttributeQuantity()}
            disabled={false}
          />
        )}
      </StyledButtonContainer>
    </StyledCard>
  );
});
