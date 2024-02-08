import { FieldValues, UseFormRegister } from 'react-hook-form';
import {
	StyledErrorLabel,
	StyledInputText,
	StyledInputTextContainer,
	StyledInputTextLabel,
} from './styled.input-text';

export interface InputTextProps {
	register: UseFormRegister<FieldValues>;
	name: string;
	label: string;
	isRequired: boolean;
	errorLabel?: string;
}

export const InputText = ({
	register,
	name,
	label,
	isRequired,
	errorLabel,
}: InputTextProps) => {
	return (
		<StyledInputTextContainer>
			<StyledInputTextLabel>{label}</StyledInputTextLabel>
			<StyledInputText
				{...register(name, {
					required: isRequired ? `${label} is required` : false,
				})}
				$hasError={!!errorLabel}
			/>
			{!!errorLabel && <StyledErrorLabel>{errorLabel}</StyledErrorLabel>}
		</StyledInputTextContainer>
	);
};
