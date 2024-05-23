import { Box, Button, HStack, IconButton } from '@chakra-ui/react';
import { IconChevronLeft, IconChevronRight } from '@tabler/icons-react';
import { usePagination } from '@refinedev/chakra-ui';

export interface PaginationProps {
	current: number;
	pageCount: number;
	setCurrent: (page: number) => void;
}

export const Pagination = ({
	current,
	pageCount,
	setCurrent,
}: PaginationProps) => {
	const pagination = usePagination({
		current,
		pageCount,
	});

	return (
		<Box display="flex" justifyContent="flex-end">
			<HStack my="3" spacing="1">
				{pagination?.prev && (
					<IconButton
						aria-label="previous page"
						onClick={() => setCurrent(current - 1)}
						disabled={!pagination?.prev}
						variant="outline"
					>
						<IconChevronLeft size="18" />
					</IconButton>
				)}

				{pagination?.items.map((page) => {
					if (typeof page === 'string')
						return <span key={page}>...</span>;

					return (
						<Button
							key={page}
							onClick={() => setCurrent(page)}
							variant={page === current ? 'solid' : 'outline'}
						>
							{page}
						</Button>
					);
				})}
				{pagination?.next && (
					<IconButton
						aria-label="next page"
						onClick={() => setCurrent(current + 1)}
						variant="outline"
					>
						<IconChevronRight size="18" />
					</IconButton>
				)}
			</HStack>
		</Box>
	);
};
