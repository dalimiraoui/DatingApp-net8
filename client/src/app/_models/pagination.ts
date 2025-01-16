
export interface Pagination {
    currentPage: number;
    itemsPerPage: number;
    totalItems : number;
    totalPages: number;

}


export class PaginatedResul<T> {
    items ? : T 
    pagination?: Pagination
}