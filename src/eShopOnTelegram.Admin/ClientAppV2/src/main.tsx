import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App.tsx';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import { PageProducts } from './pages/page-products.tsx';
import { PageProductCategories } from './pages/page-product-categories.tsx';
import { PageLogin } from './pages/page-login.tsx';
import { withLocationStoreProvider } from './_common/components/hoc/withLocationStoreProvider.ts';
import { RouterLocationStore } from './_common/router/router-location.store.ts';
import { Container } from 'inversify';
import { DIContainer } from './di-container.ts';
import { Provider } from 'inversify-react';

const AppWithRouterLocation = withLocationStoreProvider(
	RouterLocationStore,
	App,
);

const router = createBrowserRouter([
	{
		element: <AppWithRouterLocation />,
		children: [
			{
				path: '/',
				element: <PageProducts />,
			},
			{
				path: '/products',
				element: <PageProducts />,
			},
			{
				path: '/productCategories',
				element: <PageProductCategories />,
			},
			{
				path: '/login',
				element: <PageLogin />,
			},
		],
	},
]);

const diContainer = new Container();
diContainer.load(new DIContainer());

ReactDOM.createRoot(document.getElementById('root')!).render(
	<React.StrictMode>
		<Provider container={diContainer}>
			<RouterProvider router={router} />
		</Provider>
	</React.StrictMode>,
);
