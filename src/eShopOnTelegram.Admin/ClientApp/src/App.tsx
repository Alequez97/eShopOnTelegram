import restProvider from "ra-data-simple-rest";
import { Admin, fetchUtils, Login, Resource } from "react-admin";
import ProductCategoriesList from "./components/product-categories/ProductCategoriesList";
import ProductCategoriesCreate from "./components/product-categories/ProductCategoriesCreate";
import ProductCreate from "./components/products/ProductCreate";
import { ProductsList } from "./components/products/ProductsList";
import CustomersList from "./components/customers/CustomersList";
import OrdersList from "./components/orders/OrdersList";
import { ProductCategoriesEdit } from "./components/product-categories/ProductCategoriesEdit";
import { ProductEdit } from "./components/products/ProductEdit";
import ApplicationContentEdit from "./components/application-content/ApplicationContentEdit";
import OrderDetails from "./components/orders/OrderDetails";
import { authProvider } from "./AuthProvider";
import { ACCESS_TOKEN_LOCAL_STORAGE_KEY } from "./types/auth.type";

const apiBaseUrl = import.meta.env.VITE_BACKEND_API_BASE_URL ?? "/api";

const httpClient = (url: string, options: any) => {
  if (!options.headers) {
      options.headers = new Headers({ Accept: 'application/json' });
  }
  const accessToken = localStorage.getItem(ACCESS_TOKEN_LOCAL_STORAGE_KEY);
  if (accessToken) {
    options.headers.set('Authorization', `Bearer ${accessToken}`);
  }

  return fetchUtils.fetchJson(url, options);
};

const dataProvider = restProvider(apiBaseUrl, httpClient);

function App() {
  return (
    //@ts-ignore
    <Admin dataProvider={dataProvider} loginPage={Login} authProvider={authProvider}>
      <Resource
        name="products"
        list={ProductsList}
        create={ProductCreate}
        edit={ProductEdit}
      />
      <Resource
        name="productCategories"
        options={{ label: "Product categories" }}
        list={ProductCategoriesList}
        create={ProductCategoriesCreate}
        edit={ProductCategoriesEdit}
      />
      <Resource name="customers" list={CustomersList} />
      <Resource name="orders" list={OrdersList} show={OrderDetails} />
      <Resource
        name="applicationContent"
        list={ApplicationContentEdit}
        options={{ label: "Application content" }}
      />
    </Admin>
  );
}

export default App;
