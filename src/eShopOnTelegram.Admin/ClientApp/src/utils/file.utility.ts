export const fileToBase64 = async (file: File): Promise<string | null> => {
	return new Promise<string>((resolve, reject) => {
		const reader = new FileReader();

		reader.onload = (event) => {
			if (event.target && event.target.result) {
				const base64String = event.target.result
					.toString()
					.split(',')[1];
				resolve(base64String);
			} else {
				reject();
			}
		};

		reader.onerror = () => {
			reject();
		};

		reader.readAsDataURL(file);
	});
};
