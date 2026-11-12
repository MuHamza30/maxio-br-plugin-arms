using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class ReopenInvoiceError : ApiError
{
    private readonly Optional<ErrorListResponse1> _errorListResponse1Value;

    private readonly Optional<object?> _objectValue;

    private ReopenInvoiceError(Optional<ErrorListResponse1> errorListResponse1Value,
        Optional<object?> objectValue,
        Optional<RawError> fallback) : base(fallback)
    {
        _errorListResponse1Value = errorListResponse1Value;
        _objectValue = objectValue;
    }

    private static ReopenInvoiceError AsErrorListResponse1(ErrorListResponse1 value) =>
        new(Optional<ErrorListResponse1>.Some(value), default, default);

    private static ReopenInvoiceError AsObject(object? value) => new(default, Optional<object?>.Some(value), default);

    private static ReopenInvoiceError AsFallback(RawError value) =>
        new(default, default, Optional<RawError>.Some(value));

    public bool TryGetErrorListResponse1([NotNullWhen(true)] out ErrorListResponse1? value) =>
        _errorListResponse1Value.TryGetValue(out value);

    public bool TryGetObject(out object? value) => _objectValue.TryGetValue(out value);

    public TResult Match<TResult>(Func<ErrorListResponse1, TResult> onErrorListResponse1,
        Func<object?, TResult> onObject,
        Func<RawError, TResult> otherwise) =>
        this switch
        {
            _ when TryGetErrorListResponse1(out var errorListResponse1Value) => onErrorListResponse1(
                errorListResponse1Value),
            _ when TryGetObject(out var objectValue) => onObject(objectValue),
            _ when TryGetRawError(out var fallback) => otherwise(fallback),
            _ => throw new InvalidOperationException($"{nameof(ReopenInvoiceError)} holds no value.")
        };

    public void Match(Action<ErrorListResponse1> onErrorListResponse1,
        Action<object?> onObject,
        Action<RawError> otherwise)
    {
        if (TryGetErrorListResponse1(out var errorListResponse1Value)) onErrorListResponse1(errorListResponse1Value);
        else if (TryGetObject(out var objectValue)) onObject(objectValue);
        else if (TryGetRawError(out var fallback)) otherwise(fallback);
        else throw new InvalidOperationException($"{nameof(ReopenInvoiceError)} holds no value.");
    }

    private static Task<ReopenInvoiceError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            404 => response.Json<object?>().As(AsObject),
            422 => response.Json<ErrorListResponse1>().As(AsErrorListResponse1),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<ReopenInvoiceError> Response { get; } = new(Create);
}
