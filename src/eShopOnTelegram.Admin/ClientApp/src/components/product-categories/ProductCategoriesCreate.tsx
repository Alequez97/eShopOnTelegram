import React from "react";
import { Create, required, SimpleForm, TextInput } from "react-admin";

export default function ProductCategoriesCreate() {
  return (
    <Create title="Add new product categories">
      <SimpleForm>
        <TextInput source="name" validate={required()} />
      </SimpleForm>
    </Create>
  );
}
