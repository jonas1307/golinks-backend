namespace Golinks.Application.ViewModel
{
    public class PaginationResponse
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public string FirstPage { get; set; }
        public string LastPage { get; set; }
        public string NextPage { get; set; }
        public string PreviousPage { get; set; }

        public PaginationResponse(string baseUrl, int pageNumber, int pageSize, int totalItems)
        {
            TotalItems = totalItems;
            PageNumber = pageNumber;
            PageSize = pageSize;

            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            FirstPage = $"{baseUrl}?pageNumber=1&pageSize={pageSize}";
            LastPage = $"{baseUrl}?pageNumber={totalPages}&pageSize={pageSize}";
            NextPage = pageNumber < totalPages ? $"{baseUrl}?pageNumber={pageNumber + 1}&pageSize={pageSize}" : null;
            PreviousPage = pageNumber > 1 ? $"{baseUrl}?pageNumber={pageNumber - 1}&pageSize={pageSize}" : null;
        }
    }
}
