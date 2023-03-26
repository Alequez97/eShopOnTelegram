import React from "react";
import { List, Datagrid, TextField } from "react-admin";

export default function ProductCategoriesList() {
  return (
    <List>
      <Datagrid>
        <TextField source="name" sortable={true} />
      </Datagrid>
    </List>
  );
}
