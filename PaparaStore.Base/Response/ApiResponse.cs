namespace PaparaStore.Base.Response;
public partial class ApiResponse
{
    public string Message { get; set; }
    public bool IsSuccess { get; set; }
    public DateTime ServerDate { get; set; } = DateTime.Now;
    public Guid ReferenceNumber { get; set; } = Guid.NewGuid();

    public ApiResponse(string message = null)
    {
        if (string.IsNullOrEmpty(message))
        {
            IsSuccess = true;
        }
        else
        {
            IsSuccess = false;
            Message = message;
          
        }
    }
}

public partial class ApiResponse<T>
{
    public T Data { get; set; }
    public string Message { get; set; }
    public bool IsSuccess { get; set; }

    public ApiResponse(T data)
    {
        IsSuccess = true;
        Data = data;
        Message = "Success";
    }

    public ApiResponse(bool isSuccess)
    {
        IsSuccess = isSuccess;
        Data = default;
        Message = isSuccess ? "Success" : "Error";
    }

    public ApiResponse(string error)
    {
        IsSuccess = false;
        Data = default;
        Message = error;
    }
    public ApiResponse()
    {
        IsSuccess = true;
        Data = default;
        Message = "Success";
    }
}
