namespace Booking.API.WebAPI.Utilities;

public class Result<T>
{
    protected Result(T data, bool isSuccess, IList<string> errors)
    {
        Data = data;
        IsSuccess = isSuccess;
        Errors = errors ?? new List<string>();
    }

    public T Data { get; private set; }
    public bool IsSuccess { get; private set; }
    public IList<string> Errors { get; private set; }

    public static Result<T> Success(T data)
    {
        return new Result<T>(data, true, null);
    }

    public static Result<T> Failure(string error)
    {
        var errors = new List<string> { error };
        return new Result<T>(default, false, errors);
    }

    public static Result<T> Failure(IList<string> errors)
    {
        return new Result<T>(default, false, errors);
    }
}