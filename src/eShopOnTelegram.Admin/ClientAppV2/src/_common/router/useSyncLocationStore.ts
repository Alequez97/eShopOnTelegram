import { RouterLocation } from './router-location.type';
import { useLocation, useNavigate, useParams } from 'react-router-dom';
import { useCallback, useEffect } from 'react';

export const useSyncLocationStore = (store: RouterLocation) => {
	const location = useLocation();
	const { pathname, search } = location;
	const params = useParams();
	const navigate = useNavigate();

	const syncState = useCallback(() => {
		store.setLocationProps({
			params,
			pathname,
			search,
		});
	}, [pathname, params, search, store]);

	useEffect(() => {
		syncState();
	}, [syncState]);

	useEffect(() => {
		store.setNavigate(navigate);
	}, [navigate]);
};
