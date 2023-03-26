import React from "react";
import { List, Datagrid, TextField, ReferenceField } from "react-admin";

export default function ProductCategoriesList(props: any) {
  return (
    <List {...props}>
      <Datagrid>
        {/* <ReferenceField source="id" reference="users" label="Name"> */}
        <TextField source="name" sortable={true} />
      </Datagrid>
    </List>
  );
}
