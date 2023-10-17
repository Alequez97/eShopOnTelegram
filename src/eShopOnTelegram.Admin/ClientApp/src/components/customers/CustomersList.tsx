import { List, Datagrid, TextField, SimpleList } from 'react-admin';
import { useMediaQuery } from 'react-responsive';
import { Customer } from '../../types/api-response.type';

export default function CustomersList() {
	const isMobile = useMediaQuery({ query: `(max-width: 760px)` });

	return isMobile ? (
		<List>
			<SimpleList
				primaryText={(customer: Customer) =>
					`${customer.firstName} ${
						customer.lastName ? customer.lastName : ''
					}`
				}
				secondaryText={(customer: Customer) =>
					customer.username ? customer.username : ''
				}
				rowSx={() => ({ border: '1px solid #eee' })}
				tertiaryText={(customer: Customer) => customer.telegramUserUID}
			/>
		</List>
	) : (
		<List>
			<Datagrid bulkActionButtons={false}>
				<TextField
					source="telegramUserUID"
					label="Telegram User UID"
					sortable={false}
				/>
				<TextField source="username" sortable={false} emptyText={'-'} />
				<TextField source="firstName" sortable={false} />
				<TextField source="lastName" sortable={false} emptyText={'-'} />
			</Datagrid>
		</List>
	);
}
