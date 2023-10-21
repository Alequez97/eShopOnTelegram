import {
	StyledBrowser,
	StyledBrowserButton,
	StyledControls,
	StyledErrorMessageContainer,
	StyledEye,
	StyledLips,
	StyledMouth,
} from './error.styled';

export const Error = () => {
	return (
		<>
			<StyledBrowser>
				<StyledControls>
					<StyledBrowserButton type="close" />
					<StyledBrowserButton type="minimize" />
					<StyledBrowserButton type="maximize" />
				</StyledControls>

				<StyledEye />
				<StyledEye />
				<StyledMouth>
					<StyledLips />
					<StyledLips />
					<StyledLips />
					<StyledLips />
				</StyledMouth>
			</StyledBrowser>
			<StyledErrorMessageContainer>
				<p>
					Unfortunately, we&apos;re unable to fulfill your request. Try
					your request again and if the error continues, please
					contact <a href="#">support team</a>.
				</p>
			</StyledErrorMessageContainer>
		</>
	);
};
