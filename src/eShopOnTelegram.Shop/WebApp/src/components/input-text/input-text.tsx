import { FieldValues, UseFormRegister } from 'react-hook-form';
import {
	StyledErrorLabel,
	StyledInputText,
	StyledInputTextContainer,
	StyledInputTextLabel,
} from './styled.input-text';
import { useTranslation } from 'react-i18next';

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
	const { t } = useTranslation();

	return (
		<StyledInputTextContainer>
			<StyledInputTextLabel>{label}</StyledInputTextLabel>
			<StyledInputText
				{...register(name, {
					required: isRequired
						? `${label} ${t('is_required')}`
						: false,
				})}
				$hasError={!!errorLabel}
			/>
			{!!errorLabel && <StyledErrorLabel>{errorLabel}</StyledErrorLabel>}
		</StyledInputTextContainer>
	);
};
