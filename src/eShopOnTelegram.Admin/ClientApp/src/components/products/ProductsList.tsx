import { List, Datagrid, TextField, EditButton } from "react-admin";

export default function ProductsList() {
  return (
    <List>
      <Datagrid>
        <TextField source="productName" sortable={false} />
        <TextField source="productCategoryName" sortable={false} />
        <TextField source="originalPrice" sortable={false} />
        <TextField
          source="priceWithDiscount"
          sortable={false}
          emptyText={"-"}
        />
        <TextField source="quantityLeft" sortable={false} />
        <EditButton />
      </Datagrid>
    </List>
  );
}
