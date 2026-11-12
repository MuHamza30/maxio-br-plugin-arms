using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models.AnyOf;

namespace MaxioAdvancedBilling.Errors;

public sealed class CreatePrepaymentError : ApiError
{
    private readonly Optional<CreatePrepaymentErrorResponse> _createPrepaymentErrorResponseValue;

    private CreatePrepaymentError(Optional<CreatePrepaymentErrorResponse> createPrepaymentErrorResponseValue,
        Optional<RawError> fallback) : base(fallback)
    {
        _createPrepaymentErrorResponseValue = createPrepaymentErrorResponseValue;
    }

    private static CreatePrepaymentError AsCreatePrepaymentErrorResponse(CreatePrepaymentErrorResponse value) =>
        new(Optional<CreatePrepaymentErrorResponse>.Some(value), default);

    private static CreatePrepaymentError AsFallback(RawError value) => new(default, Optional<RawError>.Some(value));

    public bool TryGetCreatePrepaymentErrorResponse([NotNullWhen(true)] out CreatePrepaymentErrorResponse? value) =>
        _createPrepaymentErrorResponseValue.TryGetValue(out value);

    public TResult Match<TResult>(Func<CreatePrepaymentErrorResponse, TResult> onCreatePrepaymentErrorResponse,
        Func<RawError, TResult> otherwise) =>
        this switch
        {
            _ when TryGetCreatePrepaymentErrorResponse(
                out var createPrepaymentErrorResponseValue) => onCreatePrepaymentErrorResponse(
                createPrepaymentErrorResponseValue),
            _ when TryGetRawError(out var fallback) => otherwise(fallback),
            _ => throw new InvalidOperationException($"{nameof(CreatePrepaymentError)} holds no value.")
        };

    public void Match(Action<CreatePrepaymentErrorResponse> onCreatePrepaymentErrorResponse, Action<RawError> otherwise)
    {
        if (TryGetCreatePrepaymentErrorResponse(
            out var createPrepaymentErrorResponseValue)) onCreatePrepaymentErrorResponse(
            createPrepaymentErrorResponseValue);
        else if (TryGetRawError(out var fallback)) otherwise(fallback);
        else throw new InvalidOperationException($"{nameof(CreatePrepaymentError)} holds no value.");
    }

    private static Task<CreatePrepaymentError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            422 => response.Json<CreatePrepaymentErrorResponse>().As(AsCreatePrepaymentErrorResponse),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<CreatePrepaymentError> Response { get; } = new(Create);
}
