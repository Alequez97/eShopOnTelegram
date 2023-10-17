import {
	Datagrid,
	DateField,
	List,
	ShowButton,
	SimpleList,
	TextField,
} from 'react-admin';
import { useMediaQuery } from 'react-responsive';
import { Order } from '../../types/api-response.type';

export default function OrdersList() {
	const isMobile = useMediaQuery({ query: `(max-width: 760px)` });

	return isMobile ? (
		<List>
			<SimpleList
				primaryText={(order: Order) => `${order.orderNumber}`}
				secondaryText={(order: Order) =>
					`Total price: ${order.totalPrice}`
				}
				tertiaryText={(order: Order) => order.status}
				linkType={'show'}
				rowSx={() => ({ border: '1px solid #eee' })}
			/>
		</List>
	) : (
		<List>
			<Datagrid bulkActionButtons={false}>
				<TextField source="orderNumber" sortable={false} />
				<DateField
					source="creationDate"
					sortable={false}
					showTime={true}
				/>
				<DateField
					source="paymentDate"
					sortable={true}
					showTime={true}
					emptyText={'-'}
				/>
				<TextField source="status" sortable={false} />
				<ShowButton label={'Order details'} />
			</Datagrid>
		</List>
	);
}
