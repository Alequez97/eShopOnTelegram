import { useContext, useEffect, useId } from 'react';
import { telegramContext } from '../telegram-context';
import { useTelegramWebApp } from '../hooks/useTelegramWebApp';
import { useSmoothButtonsTransition } from '../hooks/useSmoothButtonTransition';

export interface TelegramMainButtonProps {
	text: string;
	isDisabled: boolean;
	progress?: boolean;
	color?: string;
	textColor?: string;
	onClick?: () => void;
}

export const TelegramMainButton = ({
	text,
	isDisabled,
	progress = false,
	color,
	textColor,
	onClick,
}: TelegramMainButtonProps) => {
	const system = useContext(telegramContext);
	const buttonId = useId();
	const WebApp = useTelegramWebApp();
	const MainButton = WebApp?.MainButton;
	const themeParams = WebApp?.themeParams;

	useEffect(() => {
		MainButton?.setParams({
			color: color || themeParams?.button_color || '#fff',
		});
	}, [color, themeParams, MainButton]);

	useEffect(() => {
		MainButton?.setParams({
			text_color: textColor || themeParams?.button_text_color || '#000',
		});
	}, [MainButton, themeParams, textColor]);

	useEffect(() => {
		MainButton?.setText(text);
	}, [text, MainButton]);

	useEffect(() => {
		if (isDisabled) {
			MainButton?.hide();
		} else {
			MainButton?.show();
		}
	}, [isDisabled, MainButton]);

	useEffect(() => {
		if (progress) {
			MainButton?.showProgress(false);
		} else if (!progress) {
			MainButton?.hideProgress();
		}
	}, [progress, MainButton]);

	useEffect(() => {
		if (!onClick) {
			return;
		}

		MainButton?.onClick(onClick);
		return () => {
			MainButton?.offClick(onClick);
		};
	}, [onClick, MainButton]);

	useSmoothButtonsTransition({
		show: MainButton?.show,
		hide: MainButton?.hide,
		currentShowedIdRef: system.MainButton,
		id: buttonId,
	});

	return <p>{isDisabled ? 'true' : 'false'}</p>;
};
