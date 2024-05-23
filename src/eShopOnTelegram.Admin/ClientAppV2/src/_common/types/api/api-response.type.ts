export interface ApiResponse<T> {
	data: T;
	metadata: ResponseMetadata;
}

export interface ResponseMetadata {
	pageNumber: number;
	itemsPerPage: number;
	totalItemsInDatabase: number;
	totalPages: number;
}
