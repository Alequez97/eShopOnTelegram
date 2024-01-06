import {
	List,
	Datagrid,
	TextField,
	EditButton,
	ArrayField,
	ChipField,
	SimpleList,
	TextInput,
} from 'react-admin';
import { useMediaQuery } from 'react-responsive';

export const ProductsList = () => {
	const productFilter = [
		<TextInput
			label="Product name"
			source="name"
			key={'productNameFilter'}
		/>,
		<TextInput
			label="Product category name"
			source="productCategoryName"
			key={'productCategoryNameFilter'}
		/>,
	];
	const isMobile = useMediaQuery({ query: `(max-width: 760px)` });

	return (
		<List filters={productFilter}>
			{isMobile ? (
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
					rowSx={() => ({ border: '1px solid #eee' })}
				/>
			) : (
				<Datagrid>
					<TextField source="name" sortable={false} />
					<TextField source="productCategoryName" sortable={false} />
					<ArrayField source="productAttributes" sortable={false}>
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
			)}
		</List>
	);
};
