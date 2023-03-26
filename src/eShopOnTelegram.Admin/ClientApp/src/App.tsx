import restProvider from "ra-data-simple-rest";
import { Admin, ListGuesser, Resource } from "react-admin";
import ProductCategoriesList from "./components/product-categories/ProductCategoriesList";
import ProductCategoriesCreate from "./components/product-categories/ProductCategoriesCreate";
import ProductCreate from "./components/products/ProductCreate";
import ProductsList from "./components/products/ProductsList";

const apiBaseUrl = import.meta.env.VITE_BACKEND_API_BASE_URL ?? "";
const dataProvider = restProvider(apiBaseUrl);

function App() {
  return (
    <Admin dataProvider={dataProvider}>
      <Resource name="products" list={ProductsList} create={ProductCreate} />
      <Resource
        name="productCategories"
        list={ProductCategoriesList}
        create={ProductCategoriesCreate}
      />
    </Admin>
  );
}

export default App;
