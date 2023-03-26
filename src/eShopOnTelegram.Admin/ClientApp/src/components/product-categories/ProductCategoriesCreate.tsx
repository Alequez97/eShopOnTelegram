import React from "react";
import { Create, required, SimpleForm, TextInput } from "react-admin";

export default function ProductCategoriesCreate(props: any) {
  return (
    <Create title="Add new product categories" {...props}>
      <SimpleForm>
        <TextInput source="name" validate={required()} />
      </SimpleForm>
    </Create>
  );
}
