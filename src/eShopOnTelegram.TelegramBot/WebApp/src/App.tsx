import { BrowserRouter, Route, Routes } from "react-router-dom";
import { Products } from "./pages/Products/products";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route index element={<Products />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
