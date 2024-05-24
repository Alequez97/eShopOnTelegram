import { Spinner } from '@chakra-ui/react';

export const Loader = () => {
	return (
		<div
			style={{
				height: '100vh',
				display: 'flex',
				justifyContent: 'center',
				alignItems: 'center',
			}}
		>
			<Spinner />
		</div>
	);
};
