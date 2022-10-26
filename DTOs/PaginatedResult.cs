namespace LibraryRegister.DTOs
{
	public class PaginatedResult<T>
	{
		public PaginatedResult(PaginatedList<T> paginatedList) {
			List = paginatedList;
			HasPreviousPage = paginatedList.PageIndex > 1;
			HasNextPage = paginatedList.PageIndex < paginatedList.TotalPages;
		}

		public PaginatedList<T> List { get; set; }
		public bool HasPreviousPage { get; private set; }
		public bool HasNextPage { get; private set; }
	}
}
