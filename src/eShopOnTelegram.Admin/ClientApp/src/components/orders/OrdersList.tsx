import { List, Datagrid, TextField } from "react-admin";

export default function OrdersList() {
  return (
    <List>
      <Datagrid>
        <TextField source="orderNumber" sortable={false} />
        <TextField source="creationDate" sortable={false} />
        <TextField source="paymentDate" sortable={false} emptyText={"-"} />
        <TextField source="status" sortable={false} />
      </Datagrid>
    </List>
  );
}
