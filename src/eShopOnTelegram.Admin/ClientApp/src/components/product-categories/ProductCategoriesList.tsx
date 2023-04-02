import React from "react";
import { List, Datagrid, TextField, EditButton } from "react-admin";

export default function ProductCategoriesList() {
  return (
    <List>
      <Datagrid>
        <TextField source="name" sortable={true} />
        <EditButton />
      </Datagrid>
    </List>
  );
}
