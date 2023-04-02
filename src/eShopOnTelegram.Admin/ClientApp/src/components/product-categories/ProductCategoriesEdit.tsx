import { Edit, required, SimpleForm, TextInput } from "react-admin";

export default function ProductCategoriesEdit() {
  return (
    <Edit title="Edit product category">
      <SimpleForm>
        <TextInput disabled source="id" />
        <TextInput source="name" validate={required()} />
      </SimpleForm>
    </Edit>
  );
}
