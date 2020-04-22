export interface Pagination {
  currentPage: number;
  itemsPerPage: number;
  totalItems: number;
  totalPages: number;
}

// We get back Users and Pagination from API
// Generic pagination results to be stored on T
export class PaginatedResult<T> {
  result: T;
  pagination: Pagination;
}
