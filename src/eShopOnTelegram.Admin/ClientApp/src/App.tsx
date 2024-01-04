import restProvider from 'ra-data-simple-rest';
import {
	Admin,
	fetchUtils,
	HttpError,
	Login,
	Options,
	Resource,
} from 'react-admin';
import ProductCategoriesList from './components/product-categories/ProductCategoriesList';
import ProductCategoriesCreate from './components/product-categories/ProductCategoriesCreate';
import ProductCreate from './components/products/ProductCreate';
import { ProductsList } from './components/products/ProductsList';
import CustomersList from './components/customers/CustomersList';
import OrdersList from './components/orders/OrdersList';
import { ProductCategoriesEdit } from './components/product-categories/ProductCategoriesEdit';
import { ProductEdit } from './components/products/ProductEdit';
import ApplicationContentEdit from './components/application-content/ApplicationContentEdit';
import OrderDetails from './components/orders/OrderDetails';
import { authProvider } from './AuthProvider';
import { ACCESS_TOKEN_LOCAL_STORAGE_KEY } from './types/auth.type';
import { refreshAccessToken } from './utils/auth.utility';
import { UserCreate } from './components/users/UserCreate';
import { SecretsCreate } from './components/secrets/SecretsCreate';

const apiBaseUrl = import.meta.env.VITE_BACKEND_API_BASE_URL ?? '/api';

const httpClient = async (url: string, options: Options = {}) => {
	if (!options.headers) {
		options.headers = new Headers({ Accept: 'application/json' });
	}

	const accessToken = localStorage.getItem(ACCESS_TOKEN_LOCAL_STORAGE_KEY);
	if (accessToken) {
		(options.headers as Headers).set(
			'Authorization',
			`Bearer ${accessToken}`,
		);
	}

	try {
		const response = await fetchUtils.fetchJson(url, options);
		return Promise.resolve(response);
	} catch (error: unknown) {
		if (error instanceof HttpError && error?.status === 401) {
			try {
				const newAccessToken = await refreshAccessToken();
				(options.headers as Headers).set(
					'Authorization',
					`Bearer ${newAccessToken}`,
				);

				// Retry the original request with the new access token
				const responseWithNewAccessToken = await fetchUtils.fetchJson(
					url,
					options,
				);

				return Promise.resolve(responseWithNewAccessToken);
			} catch (refreshError) {
				// Handle refresh token failure (e.g., redirect to login)
				return Promise.reject(refreshError);
			}
		}

		// Handle other errors (e.g., network issues)
		return Promise.reject(error);
	}
};

const dataProvider = restProvider(apiBaseUrl, httpClient);

function App() {
	return (
		<Admin
			dataProvider={dataProvider}
			loginPage={Login}
			authProvider={authProvider}
		>
			<Resource
				name="products"
				list={ProductsList}
				create={ProductCreate}
				edit={ProductEdit}
			/>
			<Resource
				name="productCategories"
				options={{ label: 'Product categories' }}
				list={ProductCategoriesList}
				create={ProductCategoriesCreate}
				edit={ProductCategoriesEdit}
			/>
			<Resource name="customers" list={CustomersList} />
			<Resource name="orders" list={OrdersList} show={OrderDetails} />
			<Resource
				name="applicationContent"
				list={ApplicationContentEdit}
				options={{ label: 'Application content' }}
			/>
			<Resource
				name="users"
				list={UserCreate}
				options={{ label: 'Create user' }}
			/>
			<Resource
				name="secrets"
				list={SecretsCreate}
				options={{ label: 'Secrets' }}
			/>
		</Admin>
	);
}

export default App;
