import {
  ArrayInput,
  Create,
  FileInput,
  ImageInput,
  number,
  NumberInput,
  ReferenceInput,
  required,
  SelectInput,
  SimpleForm,
  SimpleFormIterator,
  TextInput,
  useNotify,
  useRedirect,
  useRefresh,
  useResetStore,
} from "react-admin";
import { shouldBeLessThanOriginalPrice } from "./validations/PriceWithDiscounts";
import axios from "axios";
import { useRef } from "react";
import { useForm } from "react-final-form";

function ProductCreate() {
  const formRef = useRef<HTMLDivElement | null>(null);
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

  async function fileToBase64(file: File): Promise<string | null> {
    return new Promise<string | null>((resolve) => {
      const reader = new FileReader();

      reader.onload = (event) => {
        if (event.target && event.target.result) {
          const base64String = event.target.result.toString().split(",")[1];
          resolve(base64String);
        } else {
          resolve(null);
        }
      };

      reader.onerror = () => {
        resolve(null);
      };

      reader.readAsDataURL(file);
    });
  }

  const notify = useNotify();
  const redirect = useRedirect();

  async function replaceEmptyKeysWithNull(obj: any) {
    for (const key in obj) {
      console.log("key", key);
      if (typeof obj[key] === "string" && obj[key].trim() === "") {
        obj[key] = null;
      }
    }
  }

  async function handleSubmit(request: any) {
    try {
      for (let index = 0; index < request.productAttributes.length; index++) {
        const productAttribute = request.productAttributes[index];

        const imageAsBase64 = await fileToBase64(
          productAttribute.productImage.rawFile
        );

        request.productAttributes[index].imageAsBase64 = imageAsBase64;
        request.productAttributes[index].imageName =
          productAttribute.productImage.rawFile.name;

        replaceEmptyKeysWithNull(request.productAttributes[index]);
      }
      console.log(request);
      await axios.post("/products", request);
      notify("New product created", { type: "success" });
      redirect('/products')
    } catch (error) {
      notify("Error saving application content data", { type: "error" });
    }
  }

  return (
    <SimpleForm onSubmit={handleSubmit} sanitizeEmptyValues={true}>
      <ReferenceInput
        source="productCategoryId"
        reference="productCategories"
        validate={[required()]}
      >
        <SelectInput optionText="name" />
      </ReferenceInput>
      <TextInput source="name" label="Product name" validate={[required()]} />
      <div>
        <p>
          We support adding color and size as additional properties for your
          products. If your product does not require this additional information
          just leave it empty
        </p>
      </div>
      <ArrayInput
        source="productAttributes"
        validate={[required("At least one product attribute is required")]}
      >
        <SimpleFormIterator inline disableReordering>
          <NumberInput
            source="originalPrice"
            label="Original Price"
            validate={[required("Original price is required")]}
          />
          <NumberInput
            source="priceWithDiscount"
            label="Price With Discount"
            defaultValue={null}
          />
          <NumberInput
            source="quantityLeft"
            label="Quantity Left"
            validate={[required("Quantity is required")]}
          />
          <TextInput source="color" label="Color" />
          <TextInput source="size" label="Size" />
          <FileInput
            source="productImage"
            label="Product Image"
            accept="image/*"
            validate={[required("Image is required")]}
          />
        </SimpleFormIterator>
      </ArrayInput>
    </SimpleForm>
  );
}

export default ProductCreate;
