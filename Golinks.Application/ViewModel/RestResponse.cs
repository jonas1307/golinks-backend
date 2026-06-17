namespace Golinks.Application.ViewModel;

public class RestResponse<T> where T : class
{
    public bool IsSuccess { get; set; }
    public IEnumerable<string> Errors { get; set; }
    public T Data { get; set; }
    public PaginationResponse Pagination { get; set; }

    public static RestResponse<T> Success(T data, string baseUrl, int pageNumber, int pageSize, int totalItems)
    {
        var pagination = new PaginationResponse(baseUrl, pageNumber, pageSize, totalItems);

        return new RestResponse<T> { IsSuccess = true, Data = data, Pagination = pagination };
    }

    public static RestResponse<T> Success(T data)
    {
        return new RestResponse<T> { IsSuccess = true, Data = data };
    }

    public static RestResponse<T> Error(string errorMessage)
    {
        return new RestResponse<T> { IsSuccess = false, Errors = [errorMessage] };
    }

    public static RestResponse<T> Error(IEnumerable<string> errors)
    {
        return new RestResponse<T> { IsSuccess = false, Errors = errors };
    }
}