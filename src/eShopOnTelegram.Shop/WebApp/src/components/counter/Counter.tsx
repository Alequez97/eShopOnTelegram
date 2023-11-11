import {
	CounterButton,
	StyledCounterContainer,
	StyledCounterValueBadge,
} from './counter.styled';
import { useState } from 'react';

export interface CounterProps {
	showAddButton: boolean;
	addButtonDisabled?: boolean;
	showPlusButton: boolean;
	showMinusButton: boolean;
	onAdd?: () => void;
	onRemove?: () => void;
}

export const Counter = ({
	showAddButton,
	addButtonDisabled = false,
	showPlusButton,
	showMinusButton,
	onAdd,
	onRemove,
}: CounterProps) => {
	const [value, setValue] = useState(0);

	const increaseValue = () => {
		setValue((prevValue) => prevValue + 1);
		if (onAdd) {
			onAdd();
		}
	};

	const decreaseValue = () => {
		if (value > 0) {
			setValue((prevValue) => prevValue - 1);
			if (onRemove) {
				onRemove();
			}
		}
	};

	return (
		<StyledCounterContainer>
			{showAddButton && (
				<CounterButton
					title={'Add'}
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
