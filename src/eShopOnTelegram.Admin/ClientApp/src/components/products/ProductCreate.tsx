import {
  Create,
  FileInput,
  maxLength,
  minLength,
  minValue,
  number,
  NumberInput,
  regex,
  required,
  SimpleForm,
  TextInput,
} from "react-admin";

function ProductCreate(props: any) {
  const validateCardNumber = [
    required(),
    number(),
    minLength(16),
    maxLength(16),
  ];
  const validateCvc = [required(), number(), minLength(3), maxLength(3)];
  const validateExpirationDate = regex(
    /^(0[1-9]|1[0-2])\/(\d{2})$/,
    "Invalid value or date format. Must be MM/YY"
  );
  const validateInitialWorth = [required(), number(), minValue(1)];

  return (
    <Create title="Add new product categories" {...props}>
      <SimpleForm>
        <TextInput source="productName" validate={validateCardNumber} />
        <TextInput source="productCategory" validate={validateCvc} />
        <TextInput source="originalPrice" validate={validateExpirationDate} />
        <NumberInput source="priceWithDiscount" validate={validateInitialWorth} />
        <NumberInput source="quantityLeft" validate={validateInitialWorth} />
        <FileInput source="productImage" accept="image/*" />
      </SimpleForm>
    </Create>
  );
}

export default ProductCreate;
