using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class UpdateMetafieldError : ApiError
{
    private readonly Optional<SingleErrorResponse1> _singleErrorResponse1Value;

    private UpdateMetafieldError(Optional<SingleErrorResponse1> singleErrorResponse1Value,
        Optional<RawError> fallback) : base(fallback)
    {
        _singleErrorResponse1Value = singleErrorResponse1Value;
    }

    private static UpdateMetafieldError AsSingleErrorResponse1(SingleErrorResponse1 value) =>
        new(Optional<SingleErrorResponse1>.Some(value), default);

    private static UpdateMetafieldError AsFallback(RawError value) => new(default, Optional<RawError>.Some(value));

    public bool TryGetSingleErrorResponse1([NotNullWhen(true)] out SingleErrorResponse1? value) =>
        _singleErrorResponse1Value.TryGetValue(out value);

    public TResult Match<TResult>(Func<SingleErrorResponse1, TResult> onSingleErrorResponse1,
        Func<RawError, TResult> otherwise) =>
        this switch
        {
            _ when TryGetSingleErrorResponse1(out var singleErrorResponse1Value) => onSingleErrorResponse1(
                singleErrorResponse1Value),
            _ when TryGetRawError(out var fallback) => otherwise(fallback),
            _ => throw new InvalidOperationException($"{nameof(UpdateMetafieldError)} holds no value.")
        };

    public void Match(Action<SingleErrorResponse1> onSingleErrorResponse1, Action<RawError> otherwise)
    {
        if (TryGetSingleErrorResponse1(out var singleErrorResponse1Value)) onSingleErrorResponse1(
            singleErrorResponse1Value);
        else if (TryGetRawError(out var fallback)) otherwise(fallback);
        else throw new InvalidOperationException($"{nameof(UpdateMetafieldError)} holds no value.");
    }

    private static Task<UpdateMetafieldError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            422 => response.Json<SingleErrorResponse1>().As(AsSingleErrorResponse1),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<UpdateMetafieldError> Response { get; } = new(Create);
}
