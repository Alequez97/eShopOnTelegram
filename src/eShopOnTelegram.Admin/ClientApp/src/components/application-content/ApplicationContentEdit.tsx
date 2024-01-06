import React, { useEffect, useRef, useState } from 'react';
import { SimpleForm, TextInput, useNotify, useRefresh } from 'react-admin';
import { axiosGet, axiosPatch } from '../../utils/axios.utility';

type ApplicationContent = Record<string, string>;

const ApplicationContentEdit: React.FC = () => {
	const [applicationContent, setApplicationContent] =
		useState<ApplicationContent | null>(null);
	const contentWasModifiedRef = useRef(false);

	const notify = useNotify();
	const refresh = useRefresh();

	useEffect(() => {
		const fetchApplicationContent = async () => {
			const data = await axiosGet('/applicationContent');
			setApplicationContent(data);
		};
		fetchApplicationContent().catch(() =>
			notify('Network error', { type: 'error' }),
		);
	}, [notify]);

	const handleSave = async () => {
		try {
			if (applicationContent) {
				await axiosPatch('/applicationContent', applicationContent);
				notify('Application content data saved', { type: 'success' });
				refresh();
			}
		} catch (error: unknown) {
			notify('Error saving application content data', {
				type: 'error',
			});
		}
	};

	useEffect(() => {
		// Ctrl + S event handler
		const handleKeyDown = async (event: KeyboardEvent) => {
			if (event.ctrlKey && event.key === 's') {
				event.preventDefault();
				if (contentWasModifiedRef.current) {
					await handleSave();
				} else {
					notify("Data wasn't modified", { type: 'info' });
				}
			}
		};

		document.addEventListener('keydown', handleKeyDown);

		return () => {
			document.removeEventListener('keydown', handleKeyDown);
		};
	}, [handleSave]);

	const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		contentWasModifiedRef.current = true;
		const { name, value } = event.target;
		setApplicationContent((prevData) => ({
			...(prevData as ApplicationContent),
			[name]: value,
		}));
	};

	if (!applicationContent) {
		return <div>Loading...</div>;
	}

	return (
		<SimpleForm onSubmit={handleSave}>
			<p>
				In our bot we are using HTML like syntax for text editing, that
				telegram provides in their documentation. Example is welcome
				text with telegram spoiler text. More details what tags can be
				used can be found in{' '}
				<a
					href="https://core.telegram.org/bots/api#html-style"
					target="_blank"
					rel="noopener noreferrer"
				>
					Telegram documentation
				</a>
			</p>
			<br />
			{Object.entries(applicationContent)
				.filter(([key]) => key.includes('.'))
				.map(([key, value], index, array) => {
					if (
						index === 0 ||
						key.split('.')[0] !== array[index - 1][0].split('.')[0]
					) {
						const groupName = key.split('.')[0];

						return (
							<React.Fragment key={groupName}>
								<h2>{groupName}</h2>{' '}
								{/* Render the group name */}
								<TextInput
									key={key}
									source={key}
									label={key
										.split('.')[1]
										.split(/(?=[A-Z])/)
										.map((word, index) =>
											index === 0
												? word
												: word.toLowerCase(),
										)
										.join(' ')}
									defaultValue={value}
									onChange={handleInputChange}
									fullWidth
								/>
							</React.Fragment>
						);
					}

					return (
						<TextInput
							key={key}
							source={key}
							label={key
								.split('.')[1]
								.split(/(?=[A-Z])/)
								.map((word, index) =>
									index === 0 ? word : word.toLowerCase(),
								)
								.join(' ')}
							defaultValue={value}
							onChange={handleInputChange}
							fullWidth
						/>
					);
				})}
		</SimpleForm>
	);
};

export default ApplicationContentEdit;
