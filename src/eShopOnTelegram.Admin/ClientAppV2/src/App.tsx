import { Outlet } from 'react-router-dom';
import { ChakraProvider } from '@chakra-ui/react';

function App() {
	return (
		<div>
			<ChakraProvider>
				<Outlet />
			</ChakraProvider>
		</div>
	);
}

export default App;
