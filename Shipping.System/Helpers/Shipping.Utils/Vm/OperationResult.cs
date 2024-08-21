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

    public OperationResult(ResultType type, List<string> messages, string traceId = null)
    {
        Type = type;
        Messages = messages;
        TraceId = traceId;
    }

    //public OperationResult(ResultType type, List<string> messages)
    //{
    //    Type = type;
    //    Messages = messages;
    //}

    public ResultType Type { get; }
    public IReadOnlyList<string> Messages { get; }

    public string TraceId { get; set; }

    public static OperationResult Valid(List<string> messages = default, string traceId = null)
    => new(ResultType.Success, messages ?? new List<string>(), traceId);

    public static OperationResult UnValid(string traceId = null, params string[] messages)
    => new(ResultType.Failure, messages.ToList(), traceId);

    public static OperationResult UnValid(List<string> messages, ResultType resultType = ResultType.Failure, string traceId = null)
    => new(resultType, messages, traceId);

    public static OperationResult UnAuthorized(string traceId = null, params string[] messages)
    => new(ResultType.Unauthorized, messages.ToList(), traceId);

    public static OperationResult UnAuthorized(List<string> messages, ResultType resultType = ResultType.Unauthorized, string traceId = null)
    => new(resultType, messages, traceId);
}

public class OperationResult<T> : OperationResult
{
    public OperationResult(ResultType type, List<string> messages, T content, string traceId = null) :
        base(type, messages, traceId)
    {
        Content = content;
    }

    public T Content { get; }
    public static OperationResult<T> Valid(T content,
        List<string> messages = default, string traceId = null)
    => new(ResultType.Success, messages ?? new List<string>(), content, traceId);

    public static OperationResult<T> UnValid(List<string> messages, string traceId = null)
    => new(ResultType.Failure, messages, default, traceId);

    public new static OperationResult<T> UnValid(string traceId = null, params string[] messages)
    => new(ResultType.Failure, messages.ToList(), default, traceId);

    public static OperationResult<T> UnValid(ResultType resultType, string traceId = null, params string[] messages)
    => new(resultType, messages.ToList(), default, traceId);

    public static OperationResult<T> UnAuthorized(List<string> messages, string traceId = null)
   => new(ResultType.Unauthorized, messages, default, traceId);

    public new static OperationResult<T> UnAuthorized(string traceId = null, params string[] messages)
    => new(ResultType.Unauthorized, messages.ToList(), default, traceId);

    public static OperationResult<T> UnAuthorized(ResultType resultType, string traceId = null, params string[] messages)
    => new(resultType, messages.ToList(), default, traceId);

    public new static OperationResult<T> TechError(string traceId = null, params string[] messages)
        => new(ResultType.TechError, messages.ToList(), default, traceId);
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