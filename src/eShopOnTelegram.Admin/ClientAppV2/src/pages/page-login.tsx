import {
	Box,
	Button,
	Container,
	FormControl,
	FormLabel,
	Heading,
	Input,
	Stack,
} from '@chakra-ui/react';
import { useNavigate } from 'react-router-dom';
import { useInjection } from 'inversify-react';
import { AuthDataStore } from '../stores/auth.data-store.ts';
import { useState } from 'react';

export const PageLogin = () => {
	const [username, setUsername] = useState<string>('');
	const [password, setPassword] = useState<string>('');

	const { login$ } = useInjection(AuthDataStore);

	const navigate = useNavigate();

	return (
		<Container
			maxW="lg"
			py={{ base: '12', md: '24' }}
			px={{ base: '0', sm: '8' }}
		>
			<Stack spacing="8">
				<Stack spacing="6">
					<Stack spacing={{ base: '2', md: '3' }} textAlign="center">
						<Heading size={{ base: 'xs', md: 'sm' }}>
							Log in to your account
						</Heading>
					</Stack>
				</Stack>
				<Box
					py={{ base: '0', sm: '8' }}
					px={{ base: '4', sm: '10' }}
					bg={{ base: 'transparent', sm: 'bg.surface' }}
					boxShadow={{ base: 'none', sm: 'md' }}
					borderRadius={{ base: 'none', sm: 'xl' }}
				>
					<Stack spacing="6">
						<Stack spacing="5">
							<FormControl>
								<FormLabel htmlFor="login">Login</FormLabel>
								<Input
									id="login"
									type="text"
									onChange={(event) =>
										setUsername(event.target.value)
									}
								/>
							</FormControl>
							<FormControl>
								<FormLabel htmlFor="password">
									Password
								</FormLabel>
								<Input
									id="password"
									type="password"
									onChange={(event) =>
										setPassword(event.target.value)
									}
								/>
							</FormControl>
						</Stack>
						<Stack spacing="6">
							<Button
								onClick={() => {
									login$(username, password).subscribe(() =>
										navigate('/'),
									);
								}}
							>
								Log in
							</Button>
						</Stack>
					</Stack>
				</Box>
			</Stack>
		</Container>
	);
};
