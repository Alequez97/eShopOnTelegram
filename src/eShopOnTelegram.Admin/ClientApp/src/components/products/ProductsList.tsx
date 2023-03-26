import React from "react";
import { List, Datagrid, TextField } from "react-admin";

export default function ProductsList(props: any) {
  return (
    <List {...props}>
      <Datagrid>
        <TextField source="name" sortable={true} />
      </Datagrid>
    </List>
  );
}
