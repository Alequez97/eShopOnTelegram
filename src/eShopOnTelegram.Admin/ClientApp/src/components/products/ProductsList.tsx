import {
  List,
  Datagrid,
  TextField,
  EditButton,
  ArrayField,
  ChipField,
} from "react-admin";

export const ProductsList = () => {
  return (
    <List>
      <Datagrid>
        <TextField source="name" sortable={false}/>
        <TextField source="productCategoryName" sortable={false} />
        <ArrayField source="productAttributes">
          <Datagrid bulkActionButtons={false}>
            <ChipField source="color" />
            <ChipField source="size" />
            <TextField source="originalPrice" label="Price" />
            <TextField
              source="priceWithDiscount"
              label="Price with discount"
              emptyText="-"
            />
            <TextField source="quantityLeft" label="Quantity Left" />
          </Datagrid>
        </ArrayField>
        <EditButton />
      </Datagrid>
    </List>
  );
}
