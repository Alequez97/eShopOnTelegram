import { List, Datagrid, TextField, DateField } from "react-admin";

export default function OrdersList() {
  return (
    <List>
      <Datagrid>
        <TextField source="orderNumber" sortable={false} />
        <DateField source="creationDate" sortable={false} showTime={true} />
        <DateField source="paymentDate" sortable={false} showTime={true} emptyText={"-"}/>
        <TextField source="status" sortable={false} />
      </Datagrid>
    </List>
  );
}
