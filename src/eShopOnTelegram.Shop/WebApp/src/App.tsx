import { BrowserRouter, Route, Routes } from 'react-router-dom';
import { PageProducts } from './pages/products/page-products';
import { CartItemsStoreProvider } from './contexts/cart-items-store.context';
import { PageCheckout } from './pages/checkout/page-checkout';
import { RouteLocation } from './enums/route-location.enum';
import { BackendTranslationsProvider } from './contexts/translations.context';
import { PageIndex } from './pages/index/page-index';
import { PageShippingInfo } from './pages/shipping-info/page-shipping-info';
import { ClientSideTranslationsProvider } from './locale/services/client-side-translations-provider';

function App() {
	return (
		<BackendTranslationsProvider>
			<ClientSideTranslationsProvider>
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
			</ClientSideTranslationsProvider>
		</BackendTranslationsProvider>
	);
}

export default App;
