namespace Promises.Application.Common.Models;

public class BaseResponseModel<T>
{
    public bool Succeeded { get; set; }

    public string[] Errors { get; set; }

    public string FriendlyMessage { get; set; }

    public T Data { get; set; }

    public static BaseResponseModel<T> Success( T data, string friendlyMessage = "")
    {
        return new BaseResponseModel<T>
        {
            Data = data,
            FriendlyMessage = friendlyMessage,
            Succeeded = true
        };
    }

    public static BaseResponseModel<T> Failure(IEnumerable<string> errors)
    {
        return new BaseResponseModel<T>
        {
            Errors = errors.ToArray(),
            Succeeded = false
        };
    }

    public static BaseResponseModel<T> FailureSingle(string error)
    {
        return new BaseResponseModel<T>()
        {
            Errors = new []{error},
            Succeeded = false
        };
    }
}