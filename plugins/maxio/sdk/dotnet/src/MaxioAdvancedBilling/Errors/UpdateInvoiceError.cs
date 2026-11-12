using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class UpdateInvoiceError : ApiError
{
    private readonly Optional<ErrorArrayMapResponse1> _errorArrayMapResponse1Value;

    private readonly Optional<ErrorListResponse1> _errorListResponse1Value;

    private UpdateInvoiceError(Optional<ErrorArrayMapResponse1> errorArrayMapResponse1Value,
        Optional<ErrorListResponse1> errorListResponse1Value,
        Optional<RawError> fallback) : base(fallback)
    {
        _errorArrayMapResponse1Value = errorArrayMapResponse1Value;
        _errorListResponse1Value = errorListResponse1Value;
    }

    private static UpdateInvoiceError AsErrorArrayMapResponse1(ErrorArrayMapResponse1 value) =>
        new(Optional<ErrorArrayMapResponse1>.Some(value), default, default);

    private static UpdateInvoiceError AsErrorListResponse1(ErrorListResponse1 value) =>
        new(default, Optional<ErrorListResponse1>.Some(value), default);

    private static UpdateInvoiceError AsFallback(RawError value) =>
        new(default, default, Optional<RawError>.Some(value));

    public bool TryGetErrorArrayMapResponse1([NotNullWhen(true)] out ErrorArrayMapResponse1? value) =>
        _errorArrayMapResponse1Value.TryGetValue(out value);

    public bool TryGetErrorListResponse1([NotNullWhen(true)] out ErrorListResponse1? value) =>
        _errorListResponse1Value.TryGetValue(out value);

    public TResult Match<TResult>(Func<ErrorArrayMapResponse1, TResult> onErrorArrayMapResponse1,
        Func<ErrorListResponse1, TResult> onErrorListResponse1,
        Func<RawError, TResult> otherwise) =>
        this switch
        {
            _ when TryGetErrorArrayMapResponse1(out var errorArrayMapResponse1Value) => onErrorArrayMapResponse1(
                errorArrayMapResponse1Value),
            _ when TryGetErrorListResponse1(out var errorListResponse1Value) => onErrorListResponse1(
                errorListResponse1Value),
            _ when TryGetRawError(out var fallback) => otherwise(fallback),
            _ => throw new InvalidOperationException($"{nameof(UpdateInvoiceError)} holds no value.")
        };

    public void Match(Action<ErrorArrayMapResponse1> onErrorArrayMapResponse1,
        Action<ErrorListResponse1> onErrorListResponse1,
        Action<RawError> otherwise)
    {
        if (TryGetErrorArrayMapResponse1(out var errorArrayMapResponse1Value)) onErrorArrayMapResponse1(
            errorArrayMapResponse1Value);
        else if (TryGetErrorListResponse1(out var errorListResponse1Value)) onErrorListResponse1(
            errorListResponse1Value);
        else if (TryGetRawError(out var fallback)) otherwise(fallback);
        else throw new InvalidOperationException($"{nameof(UpdateInvoiceError)} holds no value.");
    }

    private static Task<UpdateInvoiceError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            404 => response.Json<ErrorListResponse1>().As(AsErrorListResponse1),
            422 => response.Json<ErrorArrayMapResponse1>().As(AsErrorArrayMapResponse1),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<UpdateInvoiceError> Response { get; } = new(Create);
}
