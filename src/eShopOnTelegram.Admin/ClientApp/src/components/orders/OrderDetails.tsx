import {
	Show,
	SimpleShowLayout,
	TextField,
	DateField,
	Datagrid,
	ArrayField,
	FunctionField,
	useNotify,
	useShowController,
	Button,
} from 'react-admin';
import { CartItem } from '../../types/orders.type';
import { Order } from '../../types/api-response.type';

export default function OrderDetails() {
	const notify = useNotify();
	const { record } = useShowController<Order>();

	const copyDeliveryInformationToClipboard = () => {
		try {
			const deliveryInformationArray = [
				record?.country,
				record?.city,
				record?.postCode,
				record?.streetLine1,
				record?.streetLine2,
			];
			const deliveryInformation = deliveryInformationArray
				.filter((deliveryInformationItem) => !!deliveryInformationItem)
				.join(', ');

			navigator.clipboard.writeText(deliveryInformation);
			notify('Copied to clipboard', { type: 'success' });
		} catch {
			console.error('Cannot copy text to clipboard');
			notify('Try again later', { type: 'error' });
		}
	};

	return (
		<Show>
			<SimpleShowLayout>
				<h2 id="order-information">Order information</h2>
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
				<h2>
					Delivery information{' '}
					<Button onClick={copyDeliveryInformationToClipboard}>
						<span style={{fontSize: '16px'}}>Copy to clipboard</span>
					</Button>
				</h2>{' '}
				<TextField source="country" label="Country" />
				<TextField source="city" />
				<TextField source="postCode" />
				<TextField source="streetLine1" />
				<TextField source="streetLine2" />
			</SimpleShowLayout>
		</Show>
	);
}
