import React, { ComponentType } from 'react';
import { interfaces } from 'inversify';
import { observer } from 'mobx-react-lite';
import { useInjection } from 'inversify-react';
import { RouterLocation } from '../../router/router-location.type.ts';
import { useSyncLocationStore } from '../../router/useSyncLocationStore.ts';

export const withLocationStoreProvider = (
	locationStoreIdentifier: interfaces.ServiceIdentifier<RouterLocation>,
	Component: ComponentType,
) =>
	observer(() => {
		{
			const locationStore = useInjection(locationStoreIdentifier);

			useSyncLocationStore(locationStore);

			return React.createElement(Component);
		}
	});
