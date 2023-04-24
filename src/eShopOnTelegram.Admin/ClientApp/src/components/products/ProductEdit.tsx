import { Edit, required, SimpleForm, TextInput } from "react-admin";

export function ProductEdit() {
  return (
    <Edit title="Edit product">
      <SimpleForm>
        <TextInput disabled source="id" />
        <TextInput source="productName" />
        <TextInput source="originalPrice" />
        <TextInput source="priceWithDiscount" />
        <TextInput source="quantityLeft" />
      </SimpleForm>
    </Edit>
  );
}
