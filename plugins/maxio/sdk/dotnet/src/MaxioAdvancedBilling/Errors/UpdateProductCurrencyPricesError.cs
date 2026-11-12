using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class UpdateProductCurrencyPricesError : ApiError
{
    private readonly Optional<ErrorArrayMapResponse1> _errorArrayMapResponse1Value;

    private UpdateProductCurrencyPricesError(Optional<ErrorArrayMapResponse1> errorArrayMapResponse1Value,
        Optional<RawError> fallback) : base(fallback)
    {
        _errorArrayMapResponse1Value = errorArrayMapResponse1Value;
    }

    private static UpdateProductCurrencyPricesError AsErrorArrayMapResponse1(ErrorArrayMapResponse1 value) =>
        new(Optional<ErrorArrayMapResponse1>.Some(value), default);

    private static UpdateProductCurrencyPricesError AsFallback(RawError value) =>
        new(default, Optional<RawError>.Some(value));

    public bool TryGetErrorArrayMapResponse1([NotNullWhen(true)] out ErrorArrayMapResponse1? value) =>
        _errorArrayMapResponse1Value.TryGetValue(out value);

    public TResult Match<TResult>(Func<ErrorArrayMapResponse1, TResult> onErrorArrayMapResponse1,
        Func<RawError, TResult> otherwise) =>
        this switch
        {
            _ when TryGetErrorArrayMapResponse1(out var errorArrayMapResponse1Value) => onErrorArrayMapResponse1(
                errorArrayMapResponse1Value),
            _ when TryGetRawError(out var fallback) => otherwise(fallback),
            _ => throw new InvalidOperationException($"{nameof(UpdateProductCurrencyPricesError)} holds no value.")
        };

    public void Match(Action<ErrorArrayMapResponse1> onErrorArrayMapResponse1, Action<RawError> otherwise)
    {
        if (TryGetErrorArrayMapResponse1(out var errorArrayMapResponse1Value)) onErrorArrayMapResponse1(
            errorArrayMapResponse1Value);
        else if (TryGetRawError(out var fallback)) otherwise(fallback);
        else throw new InvalidOperationException($"{nameof(UpdateProductCurrencyPricesError)} holds no value.");
    }

    private static Task<UpdateProductCurrencyPricesError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            422 => response.Json<ErrorArrayMapResponse1>().As(AsErrorArrayMapResponse1),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<UpdateProductCurrencyPricesError> Response { get; } = new(Create);
}
