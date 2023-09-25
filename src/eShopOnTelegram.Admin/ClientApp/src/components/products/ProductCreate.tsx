import {
  Create,
  FileInput,
  ImageInput,
  number,
  NumberInput,
  ReferenceInput,
  required,
  SelectInput,
  SimpleForm,
  TextInput,
  useNotify,
  useRedirect,
  useRefresh,
} from "react-admin";
import { shouldBeLessThanOriginalPrice } from "./validations/PriceWithDiscounts";
import axios from "axios";

function ProductCreate() {
  const validateFileExtension = (imageObject: any) => {
    const allowedExtensions = ["png", "jpeg", "jpg", "gif"];

    const fileExtension = imageObject.title.split(".").pop().toLowerCase();
    if (!allowedExtensions.includes(fileExtension)) {
      return (
        "Invalid file format. Allowed formats are " +
        allowedExtensions.join(", ")
      );
    }

    return undefined;
  };

  const notify = useNotify();
  const redirect = useRedirect();

  async function handleSubmit(request: any) {
    try {
      const formData = new FormData();
      formData.append("name", request.name);
      formData.append("productCategoryId", request.productCategoryId);
      formData.append("quantityLeft", request.quantityLeft);
      formData.append("originalPrice", request.originalPrice);
      formData.append("priceWithDiscount", request.priceWithDiscount ?? "");
      formData.append("productImage", request.productImage.rawFile);
      await axios.post("/products", formData);
      notify("New product created", { type: "success" });
      redirect('/products')
    } catch (error) {
      notify("Error saving application content data", { type: "error" });
    }
  }

  return (
    <SimpleForm onSubmit={handleSubmit}>
      <TextInput source="name" label="Product name" validate={[required()]} />
      <ReferenceInput source="productCategoryId" reference="productCategories">
        <SelectInput optionText="name" validate={[required()]} />
      </ReferenceInput>
      <NumberInput source="originalPrice" validate={[required(), number()]} />
      <NumberInput
        source="priceWithDiscount"
        validate={[number(), shouldBeLessThanOriginalPrice]}
      />
      <NumberInput source="quantityLeft" validate={[required(), number()]} />
      <FileInput
        source="productImage"
        validate={[required(), validateFileExtension]}
      />
    </SimpleForm>
  );
}

export default ProductCreate;
