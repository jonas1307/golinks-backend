public class RestResponse<T> where T : class
{
    public bool IsSuccess { get; set; }
    public string ErrorMessage { get; set; }
    public T Data { get; set; }

    public static RestResponse<T> Success(T data)
    {
        return new RestResponse<T> { IsSuccess = true, Data = data };
    }

    public static RestResponse<T> Error(string errorMessage)
    {
        return new RestResponse<T> { IsSuccess = false, ErrorMessage = errorMessage };
    }
}