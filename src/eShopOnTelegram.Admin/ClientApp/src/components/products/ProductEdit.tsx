import React from "react";
import {
    Edit,
    ArrayInput,
    SimpleFormIterator,
    NumberInput,
    TextInput,
    required, SimpleForm,
} from "react-admin";
import { shouldBeLessThanOriginalPrice } from "./validations/PriceWithDiscounts";

export function ProductEdit(props: any) {
    return (
        <Edit title="Edit product" {...props}>
            <SimpleForm>
                <TextInput disabled source="id" />
                <TextInput source="name" validate={required()} />

                {/* Add an ArrayInput for productAttributes */}
                <ArrayInput source="productAttributes" label="Product Attributes">
                    <SimpleFormIterator inline disableReordering>
                        <NumberInput source="originalPrice" label="Original Price" />
                        <NumberInput
                            source="priceWithDiscount"
                            label="Price With Discount"
                            validate={[shouldBeLessThanOriginalPrice]}
                        />
                        <NumberInput source="quantityLeft" label="Quantity Left" validate={required()} />
                        <TextInput source="color" label="Color" />
                        <TextInput source="size" label="Size" />
                    </SimpleFormIterator>
                </ArrayInput>
            </SimpleForm>
        </Edit>
    );
}