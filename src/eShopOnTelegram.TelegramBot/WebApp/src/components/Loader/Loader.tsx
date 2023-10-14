import { StyledRingLoader, StyledRingLoaderSpan } from './loader.styled';

export const Loader = () => {
	return (
		<StyledRingLoader>
			Loading
			<StyledRingLoaderSpan />
		</StyledRingLoader>
	);
};
