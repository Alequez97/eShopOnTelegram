import { BrowserRouter, Route, Routes } from 'react-router-dom';
import { Products } from './pages/Products/Products';
import { CartItemsStoreProvider } from './contexts/cart-items-store.context';
import { Checkout } from './pages/checkout/Checkout';
import { RouteLocation } from './enums/route-location.enum';
import { TranslationsProvider } from './contexts/translations.context';

function App() {
	return (
		<TranslationsProvider>
			<CartItemsStoreProvider>
				<BrowserRouter>
					<Routes>
						<Route index element={<Products />} />
						<Route
							path={RouteLocation.PRODUCTS}
							element={<Products />}
						/>
						<Route
							path={RouteLocation.CHECKOUT}
							element={<Checkout />}
						/>
					</Routes>
				</BrowserRouter>
			</CartItemsStoreProvider>
		</TranslationsProvider>
	);
}

export default App;
