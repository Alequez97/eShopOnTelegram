import { NavigateFunction } from 'react-router-dom';

export interface LocationBaseProps {
	params?: AnyObject;
	search?: AnyObject;
}

export interface LocationProps<Props extends LocationBaseProps = AnyObject> {
	params: Props['params'];
	pathname: string;
	search: string;
}

export type AnyObject = Record<string, unknown>;

export interface RouterLocation<Props extends LocationBaseProps = AnyObject> {
	setLocationProps(props: LocationProps): void;
	params: Props['params'];
	search: Props['search'];
	setNavigate(navigate: NavigateFunction): void;
	navigate: NavigateFunction;
}
