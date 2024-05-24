import { Outlet } from 'react-router-dom';
import { ChakraProvider } from '@chakra-ui/react';
import { Header } from './_common/components/header';

function App() {
	return (
		<div>
			<ChakraProvider>
				<Header />
				<Outlet />
			</ChakraProvider>
		</div>
	);
}

export default App;
