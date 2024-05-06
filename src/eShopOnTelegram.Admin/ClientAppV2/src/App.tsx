import { useState } from 'react';
import { Container } from 'inversify';
import { Provider } from 'inversify-react';
import { Outlet } from 'react-router-dom';
import { DIContainer } from './di-container.ts';

function App() {
	const [diContainer] = useState(() => {
		const _container = new Container();
		_container.load(new DIContainer());
		return _container;
	});

	return (
		<div>
			<Provider container={diContainer}>
				<Outlet />
			</Provider>
		</div>
	);
}

export default App;
