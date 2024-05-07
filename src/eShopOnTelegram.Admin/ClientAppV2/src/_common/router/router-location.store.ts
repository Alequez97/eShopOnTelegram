import qs from 'qs';
import { makeAutoObservable, runInAction } from 'mobx';
import { NavigateFunction } from 'react-router-dom';
import { injectable } from 'inversify';
import 'reflect-metadata';
import {
	AnyObject,
	LocationBaseProps,
	LocationProps,
	RouterLocation,
} from './router-location.type';

@injectable()
export class RouterLocationStore<Props extends LocationBaseProps = AnyObject>
	implements RouterLocation<Props>
{
	private locationProps: Omit<LocationProps<Props>, 'state'> = {
		params: {},
		pathname: '',
		search: '',
	};

	_navigate: NavigateFunction = () => {
		console.log('navigate');
	};

	constructor() {
		makeAutoObservable(this, undefined, { autoBind: true });
	}

	setLocationProps({ params, pathname, search }: LocationProps) {
		runInAction(() => {
			this.locationProps.params = params as Props['params'];
			this.locationProps.pathname = pathname;
			this.locationProps.search = search;
		});
	}

	get navigate() {
		return this._navigate;
	}

	setNavigate(navigate: NavigateFunction) {
		this._navigate = navigate;
	}

	get params(): Props['params'] {
		return this.locationProps.params;
	}

	get search(): Props['search'] {
		return qs.parse(this.locationProps.search || '', {
			ignoreQueryPrefix: true,
		}) as Props['search'];
	}
}
