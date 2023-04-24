import restProvider from "ra-data-simple-rest";
import { Admin, Resource } from "react-admin";
import ProductCategoriesList from "./components/product-categories/ProductCategoriesList";
import ProductCategoriesCreate from "./components/product-categories/ProductCategoriesCreate";
import ProductCreate from "./components/products/ProductCreate";
import ProductsList from "./components/products/ProductsList";
import CustomersList from "./components/customers/CustomersList";
import OrdersList from "./components/orders/OrdersList";
import ProductCategoriesEdit from "./components/product-categories/ProductCategoriesEdit";

const apiBaseUrl = import.meta.env.VITE_BACKEND_API_BASE_URL ?? "";
const dataProvider = restProvider(apiBaseUrl);

function App() {
  return (
    <Admin dataProvider={dataProvider}>
      <Resource name="products" list={ProductsList} create={ProductCreate} />
      <Resource
        name="productCategories"
        options={{ label: "Product categories" }}
        list={ProductCategoriesList}
        create={ProductCategoriesCreate}
        edit={ProductCategoriesEdit}
      />
      <Resource name="customers" list={CustomersList} />
      <Resource name="orders" list={OrdersList} />
    </Admin>
  );
}

export default App;
