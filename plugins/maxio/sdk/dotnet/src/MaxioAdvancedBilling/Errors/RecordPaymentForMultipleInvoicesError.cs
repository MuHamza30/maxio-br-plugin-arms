using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class RecordPaymentForMultipleInvoicesError : ApiError
{
    private readonly Optional<ErrorListResponse1> _errorListResponse1Value;

    private RecordPaymentForMultipleInvoicesError(Optional<ErrorListResponse1> errorListResponse1Value,
        Optional<RawError> fallback) : base(fallback)
    {
        _errorListResponse1Value = errorListResponse1Value;
    }

    private static RecordPaymentForMultipleInvoicesError AsErrorListResponse1(ErrorListResponse1 value) =>
        new(Optional<ErrorListResponse1>.Some(value), default);

    private static RecordPaymentForMultipleInvoicesError AsFallback(RawError value) =>
        new(default, Optional<RawError>.Some(value));

    public bool TryGetErrorListResponse1([NotNullWhen(true)] out ErrorListResponse1? value) =>
        _errorListResponse1Value.TryGetValue(out value);

    public TResult Match<TResult>(Func<ErrorListResponse1, TResult> onErrorListResponse1,
        Func<RawError, TResult> otherwise) =>
        this switch
        {
            _ when TryGetErrorListResponse1(out var errorListResponse1Value) => onErrorListResponse1(
                errorListResponse1Value),
            _ when TryGetRawError(out var fallback) => otherwise(fallback),
            _ => throw new InvalidOperationException($"{nameof(RecordPaymentForMultipleInvoicesError)} holds no value.")
        };

    public void Match(Action<ErrorListResponse1> onErrorListResponse1, Action<RawError> otherwise)
    {
        if (TryGetErrorListResponse1(out var errorListResponse1Value)) onErrorListResponse1(errorListResponse1Value);
        else if (TryGetRawError(out var fallback)) otherwise(fallback);
        else throw new InvalidOperationException($"{nameof(RecordPaymentForMultipleInvoicesError)} holds no value.");
    }

    private static Task<RecordPaymentForMultipleInvoicesError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            422 => response.Json<ErrorListResponse1>().As(AsErrorListResponse1),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<RecordPaymentForMultipleInvoicesError> Response { get; } = new(Create);
}
