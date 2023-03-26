import restProvider from "ra-data-simple-rest";
import { Admin, Resource, ListGuesser } from "react-admin";
import ProductCreate from "./components/products/ProductCreate";
import ProductsList from "./components/products/ProductsList";

const apiBaseUrl = import.meta.env.VITE_BACKEND_API_BASE_URL ?? "";
console.log(apiBaseUrl);
const dataProvider = restProvider(apiBaseUrl);

function App() {
  return (
    <Admin dataProvider={dataProvider}>
      <Resource
        name="products"
        list={ProductsList}
        create={ProductCreate}
      />
    </Admin>
  );
}

export default App;
