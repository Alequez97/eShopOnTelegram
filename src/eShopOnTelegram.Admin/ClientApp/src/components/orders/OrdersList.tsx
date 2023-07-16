import { Datagrid, DateField, Link, List, TextField, ShowButton, useListController } from "react-admin";

export default function OrdersList() {
  const dsfa = useListController()
  console.log(dsfa)

  return (
    <List>
      <Datagrid bulkActionButtons={false}>
        <TextField source="orderNumber" sortable={false} />
        <DateField source="creationDate" sortable={false} showTime={true} />
        <DateField
          source="paymentDate"
          sortable={false}
          showTime={true}
          emptyText={"-"}
        />
        <TextField source="status" sortable={false} />
        <ShowButton label={'Order details'} />
      </Datagrid>
    </List>
  );
}
