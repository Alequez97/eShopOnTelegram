import { List, Datagrid, TextField } from "react-admin";

export default function ProductsList() {
  return (
    <List>
      <Datagrid>
        <TextField source="productName" sortable={true} />
        <TextField source="productCategoryName" sortable={true} />
        <TextField source="originalPrice" sortable={false} />
        <TextField
          source="priceWithDiscount"
          sortable={false}
          emptyText={"-"}
        />
        <TextField source="quantityLeft" sortable={true} />
      </Datagrid>
    </List>
  );
}
