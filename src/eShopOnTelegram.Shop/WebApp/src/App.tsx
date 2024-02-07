import { BrowserRouter, Route, Routes } from 'react-router-dom';
import { PageProducts } from './pages/products/page-products';
import { CartItemsStoreProvider } from './contexts/cart-items-store.context';
import { PageCheckout } from './pages/checkout/page-checkout';
import { RouteLocation } from './enums/route-location.enum';
import { TranslationsProvider } from './contexts/translations.context';
import { PageIndex } from './pages/index/page-index';
import { PageShippingInfo } from './pages/shipping-info/page-shipping-info';

function App() {
	return (
		<TranslationsProvider>
			<CartItemsStoreProvider>
				<BrowserRouter>
					<Routes>
						<Route index element={<PageIndex />} />
						<Route
							path={RouteLocation.PRODUCTS}
							element={<PageProducts />}
						/>
						<Route
							path={RouteLocation.CHECKOUT}
							element={<PageCheckout />}
						/>
						<Route
							path={RouteLocation.SHIPPING_INFO}
							element={<PageShippingInfo />}
						/>
					</Routes>
				</BrowserRouter>
			</CartItemsStoreProvider>
		</TranslationsProvider>
	);
}

export default App;
