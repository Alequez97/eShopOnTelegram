import { MutableRefObject, useContext, useEffect } from 'react';
import { telegramOptionsContext } from '../telegram-context';

// eslint-disable-next-line @typescript-eslint/no-empty-function
const _noop = () => {};

export const useSmoothButtonsTransition = ({
	id,
	show = _noop,
	hide = _noop,
	currentShowedIdRef,
}: {
	id: string;
	show: typeof _noop | undefined;
	hide: typeof _noop | undefined;
	currentShowedIdRef: MutableRefObject<string | null>;
}) => {
	const { smoothButtonsTransition, smoothButtonsTransitionMs } = useContext(
		telegramOptionsContext,
	);

	useEffect(() => {
		show();
		currentShowedIdRef.current = id;

		return () => {
			if (smoothButtonsTransition) {
				currentShowedIdRef.current = null;
				setTimeout(() => {
					if (currentShowedIdRef.current) return;

					hide();
				}, smoothButtonsTransitionMs);
			} else {
				hide();
				currentShowedIdRef.current = null;
			}
		};
	}, [
		hide,
		id,
		currentShowedIdRef,
		show,
		smoothButtonsTransition,
		smoothButtonsTransitionMs,
	]);
};
