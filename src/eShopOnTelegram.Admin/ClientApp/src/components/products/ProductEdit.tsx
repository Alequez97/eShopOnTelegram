import { Edit, number, required, SimpleForm, TextInput } from "react-admin";

export function ProductEdit() {
  return (
    <Edit title="Edit product">
      <SimpleForm>
        <TextInput disabled source="id" />
        <TextInput source="productName" validate={[required()]} />
        <TextInput source="originalPrice" validate={[required(), number()]} />
        <TextInput
          source="priceWithDiscount"
          validate={[number()]}
        />
        <TextInput source="quantityLeft" validate={[required(), number()]} />
      </SimpleForm>
    </Edit>
  );
}
