import {
	CounterButton,
	StyledCounterContainer,
	StyledCounterValueBadge,
} from './counter.styled';
import { useTranslation } from 'react-i18next';

export interface CounterProps {
	value: number | undefined;
	showAddButton: boolean;
	addButtonDisabled?: boolean;
	showPlusButton: boolean;
	showMinusButton: boolean;
	onAdd?: () => void;
	onRemove?: () => void;
}

export const Counter = ({
	value,
	showAddButton,
	addButtonDisabled = false,
	showPlusButton,
	showMinusButton,
	onAdd,
	onRemove,
}: CounterProps) => {
	const increaseValue = () => {
		if (onAdd) {
			onAdd();
		}
	};

	const decreaseValue = () => {
		if (onRemove) {
			onRemove();
		}
	};

	const { t } = useTranslation();

	return (
		<StyledCounterContainer>
			{showAddButton && (
				<CounterButton
					title={t('add')}
					type={'add'}
					onClick={increaseValue}
					disabled={addButtonDisabled}
				/>
			)}
			{showPlusButton && (
				<CounterButton
					title={'-'}
					type={'remove'}
					onClick={decreaseValue}
					disabled={false}
				/>
			)}
			<StyledCounterValueBadge $isVisible={!showAddButton}>
				{value}
			</StyledCounterValueBadge>
			{showMinusButton && (
				<CounterButton
					title={'+'}
					type={'add'}
					onClick={increaseValue}
					disabled={false}
				/>
			)}
		</StyledCounterContainer>
	);
};
