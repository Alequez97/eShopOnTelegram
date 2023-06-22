import {
  Create,
  FileInput,
  number,
  NumberInput,
  ReferenceInput,
  required,
  SelectInput,
  SimpleForm,
  TextInput,
} from "react-admin";
import { shouldBeLessThanOriginalPrice } from "./validations/PriceWithDiscounts";

function ProductCreate() {
  return (
    <Create title="Add new product">
      <SimpleForm>
        <TextInput source="productName" validate={[required()]} />
        <ReferenceInput
          source="productCategoryId"
          reference="productCategories"
        >
          <SelectInput optionText="name" validate={[required()]} />
        </ReferenceInput>
        <NumberInput source="originalPrice" validate={[required(), number()]} />
        <NumberInput source="priceWithDiscount" validate={[number(), shouldBeLessThanOriginalPrice]} />
        <NumberInput source="quantityLeft" validate={[required(), number()]} />
        <FileInput source="productImage" accept="image/*" />
      </SimpleForm>
    </Create>
  );
}

export default ProductCreate;
