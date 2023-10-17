import {
	List,
	Datagrid,
	TextField,
	EditButton,
	ArrayField,
	ChipField,
	SimpleList,
} from 'react-admin';
import { useMediaQuery } from 'react-responsive';

export const ProductsList = () => {
	const isMobile = useMediaQuery({ query: `(max-width: 760px)` });

	return isMobile ? (
		<List>
			<SimpleList
				primaryText={(record) => record.name}
				secondaryText={(record) => `${record.productCategoryName}`}
				tertiaryText={(record) =>
					`${record.productAttributes.length} ${
						record.productAttributes.length === 1
							? 'attribute'
							: 'attributes'
					}`
				}
				linkType={(record) => (record.canEdit ? 'edit' : 'show')}
				rowSx={(record) => ({
					backgroundColor: record.nb_views >= 500 ? '#efe' : 'white',
				})}
			/>
		</List>
	) : (
		<List>
			<Datagrid>
				<TextField source="name" sortable={false} />
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
						<TextField
							source="quantityLeft"
							label="Quantity Left"
						/>
					</Datagrid>
				</ArrayField>
				<EditButton />
			</Datagrid>
		</List>
	);
};
