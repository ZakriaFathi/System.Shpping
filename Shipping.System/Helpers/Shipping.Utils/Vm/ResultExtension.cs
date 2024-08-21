using FluentResults;

namespace Shipping.Utils.Vm;

public static class ResultExtension
{

    public static OperationResult ToOperationStructResult(this Result result, string traceId = "")
    {
        if (result.IsSuccess) return OperationResult.Valid();
        return OperationResult.UnValid(messages: result.Errors.ConvertAll(x => x.Message), traceId: traceId);
    }
    public static OperationResult<T> ToOperationResult<T>(this Result<T> result, string traceId = "") where T : class
    {
        if (result.IsSuccess) return OperationResult<T>.Valid(content: result.Value, traceId: traceId);
        return OperationResult<T>.UnValid(result.Errors.ConvertAll(x => x.Message), traceId: traceId);
    }
    public static OperationResult<T> ToOperationStructResult<T>(this Result<T> result, string traceId = "") where T : struct
    {
        if (result.IsSuccess) return OperationResult<T>.Valid(content: result.Value, traceId: traceId);
        return OperationResult<T>.UnValid(messages: result.Errors.ConvertAll(x => x.Message), traceId: traceId);
    }
}