import {
  Show,
  SimpleShowLayout,
  TextField,
  DateField,
  useShowController,
  Datagrid,
  NumberField,
  List,
  ArrayField,
  FunctionField,
} from "react-admin";

export default function OrderDetails(props: any) {
  const { record } = useShowController(props);

  const calculateTotalPrice = (cartItems: any[]): number => {
    return cartItems.reduce(
      (total, item) =>
        total +
        (item.priceWithDiscount ? item.priceWithDiscount : item.originalPrice) *
          item.quantity,
      0
    );
  };

  const totalPrice = record ? calculateTotalPrice(record.cartItems) : 0;

  return (
    <Show>
      <SimpleShowLayout>
        <TextField source="orderNumber" />
        <TextField source="status" />
        <DateField source="paymentDate" showTime emptyText="-" />
        <ArrayField source="cartItems">
          <Datagrid bulkActionButtons={false}>
            <FunctionField
              label="Product"
              render={(record: any) =>
                `${record.name} (${record.categoryName}) x${record.quantity}`
              }
            />
            <FunctionField
              label="Price for one item"
              render={(record: any) => (
                <>
                  {record.priceWithDiscount
                    ? record.priceWithDiscount
                    : record.originalPrice}
                </>
              )}
            />
          </Datagrid>
        </ArrayField>
        <TextField label="Total Price (All Cart Items)" source="totalPrice" record={{ totalPrice }} />
      </SimpleShowLayout>
    </Show>
  );
}
