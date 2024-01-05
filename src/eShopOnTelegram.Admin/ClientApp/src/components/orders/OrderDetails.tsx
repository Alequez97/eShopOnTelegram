import {
	Show,
	SimpleShowLayout,
	TextField,
	DateField,
	Datagrid,
	ArrayField,
	FunctionField,
} from 'react-admin';
import { CartItem } from '../../types/orders.type';

export default function OrderDetails() {
	return (
		<Show>
			<SimpleShowLayout>
				<h2>Order information</h2>
				<TextField source="orderNumber" />
				<TextField source="status" />
				<TextField source="paymentMethod" />
				<DateField source="paymentDate" showTime emptyText="-" />
				<ArrayField source="cartItems">
					<Datagrid bulkActionButtons={false}>
						<FunctionField
							label="Category"
							render={(cartItem: CartItem) =>
								`${cartItem.productAttribute.productCategoryName}`
							}
						/>
						<FunctionField
							label="Product"
							render={(cartItem: CartItem) =>
								`${cartItem.productAttribute.productName}`
							}
						/>
						<FunctionField
							label="Quantity"
							render={(cartItem: CartItem) =>
								`${cartItem.quantity}`
							}
						/>
						<FunctionField
							label="Price for one item"
							render={(cartItem: CartItem) => (
								<>
									{cartItem.productAttribute.priceWithDiscount
										? cartItem.productAttribute
												.priceWithDiscount
										: cartItem.productAttribute
												.originalPrice}
								</>
							)}
						/>
						<FunctionField
							label="Product attributes"
							render={(cartItem: CartItem) => (
								<>
									{cartItem.productAttribute.color ||
									cartItem.productAttribute.size
										? `(${
												cartItem.productAttribute
													.color &&
												`${cartItem.productAttribute.color}, `
										  } ${cartItem.productAttribute.size})`
										: null}
								</>
							)}
						/>
					</Datagrid>
				</ArrayField>
				<TextField
					label="Total Price (All Cart Items)"
					source="totalPrice"
					style={{ fontWeight: 'bold', fontSize: 20 }}
				/>
			</SimpleShowLayout>

			<SimpleShowLayout>
				<h2>Delivery information</h2>
				<TextField source="countryIso2Code" label="Country" />
				<TextField source="city" />
				<TextField source="postCode" />
				<TextField source="streetLine1" />
				<TextField source="streetLine2" />
			</SimpleShowLayout>
		</Show>
	);
}
