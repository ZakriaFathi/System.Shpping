namespace Shipping.Utils.Vm;

public class OperationResult
{
    public enum ResultType
    {
        Success = 1,
        Failure,
        TechError,
        Unauthorized
    }

    public OperationResult(ResultType type, List<string> messages)
    {
        Type = type;
        Messages = messages;
    }

    //public OperationResult(ResultType type, List<string> messages)
    //{
    //    Type = type;
    //    Messages = messages;
    //}

    public ResultType Type { get; }
    public IReadOnlyList<string> Messages { get; }
    

    public static OperationResult Valid(List<string> messages = default)
    => new(ResultType.Success, messages ?? new List<string>());

    public static OperationResult UnValid(params string[] messages)
    => new(ResultType.Failure, messages.ToList());

    public static OperationResult UnValid(List<string> messages, ResultType resultType = ResultType.Failure)
    => new(resultType, messages);

    public static OperationResult UnAuthorized(params string[] messages)
    => new(ResultType.Unauthorized, messages.ToList());

    public static OperationResult UnAuthorized(List<string> messages, ResultType resultType = ResultType.Unauthorized)
    => new(resultType, messages);
}

public class OperationResult<T> : OperationResult
{
    public OperationResult(ResultType type, List<string> messages, T content) :
        base(type, messages)
    {
        Content = content;
    }

    public T Content { get; }
    public static OperationResult<T> Valid(T content,
        List<string> messages = default)
    => new(ResultType.Success, messages ?? new List<string>(), content);

    public static OperationResult<T> UnValid(List<string> messages)
    => new(ResultType.Failure, messages, default);

    public new static OperationResult<T> UnValid(params string[] messages)
    => new(ResultType.Failure, messages.ToList(), default);

    public static OperationResult<T> UnValid(ResultType resultType, params string[] messages)
    => new(resultType, messages.ToList(), default);

    public static OperationResult<T> UnAuthorized(List<string> messages)
   => new(ResultType.Unauthorized, messages, default);

    public new static OperationResult<T> UnAuthorized(params string[] messages)
    => new(ResultType.Unauthorized, messages.ToList(), default);

    public static OperationResult<T> UnAuthorized(ResultType resultType, params string[] messages)
    => new(resultType, messages.ToList(), default);

    public new static OperationResult<T> TechError(params string[] messages)
        => new(ResultType.TechError, messages.ToList(), default);
}

public sealed class LocalizedMessage
{
    public enum Langs
    {
        Ar = 1,
        En,
        Fr,
        Es
    }

    private LocalizedMessage()
    {
    }

    public bool State { get; set; }
    public string Message { get; set; }
    public bool IsLocalized { get; set; }
    public object ObjParams { get; set; }
    public Langs Lang { get; set; }

    public static LocalizedMessage From(string message, bool isLocalized = false, object objParams = null, Langs langs = Langs.Ar, bool state = false)
    {
        return new LocalizedMessage
        {
            Message = message,
            IsLocalized = isLocalized,
            ObjParams = objParams,
            Lang = langs,
            State = state
        };
    }
}