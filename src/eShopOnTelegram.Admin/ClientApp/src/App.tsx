import restProvider from "ra-data-simple-rest";
import { Admin, Resource, ListGuesser } from "react-admin";

const apiBaseUrl = import.meta.env.VITE_BACKEND_API_BASE_URL ?? "";
console.log(apiBaseUrl);
const dataProvider = restProvider(apiBaseUrl);

function App() {
  return (
    <Admin dataProvider={dataProvider}>
      <Resource name="weather" list={ListGuesser} />
    </Admin>
  );
}

export default App;
