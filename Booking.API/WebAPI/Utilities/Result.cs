namespace Booking.API.WebAPI.Utilities;


public class Result
{
    protected Result(bool isSuccess, IList<string> errors)
    {
        IsSuccess = isSuccess;
        Errors = errors ?? new List<string>();
    }

    public bool IsSuccess { get; private set; }
    public IList<string> Errors { get; private set; }

    public static Result Success()
    {
        return new Result(true, null);
    }

    public static Result Failure(string error)
    {
        var errors = new List<string> { error };
        return new Result(false, errors);
    }

    public static Result Failure(IList<string> errors)
    {
        return new Result(false, errors);
    }
}

public class Result<T> : Result
{
    protected Result(T data, bool isSuccess, IList<string> errors)
        : base(isSuccess, errors)
    {
        Data = data;
    }

    public T Data { get; private set; }

    public static Result<T> Success(T data)
    {
        return new Result<T>(data, true, null);
    }

    public static new Result<T> Failure(string error)
    {
        var errors = new List<string> { error };
        return new Result<T>(default(T), false, errors);
    }

    public static new Result<T> Failure(IList<string> errors)
    {
        return new Result<T>(default(T), false, errors);
    }
}