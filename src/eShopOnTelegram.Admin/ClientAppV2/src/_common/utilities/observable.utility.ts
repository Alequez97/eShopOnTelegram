import { Observable } from 'rxjs';
import { reaction } from 'mobx';

export const toObservable = <T extends any | undefined>(
	obj: () => T,
	fireImmediately = true,
) => {
	return new Observable<T>(function (subscriber) {
		const disposer = reaction(
			obj,
			(value) => {
				subscriber.next(value);
			},
			{ fireImmediately },
		);

		return () => disposer();
	});
};
