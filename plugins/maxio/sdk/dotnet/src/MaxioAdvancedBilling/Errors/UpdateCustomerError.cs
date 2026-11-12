using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class UpdateCustomerError : ApiError
{
    private readonly Optional<CustomerErrorResponse1> _customerErrorResponse1Value;

    private readonly Optional<RawError> _noContentValue;

    private UpdateCustomerError(Optional<CustomerErrorResponse1> customerErrorResponse1Value,
        Optional<RawError> noContentValue,
        Optional<RawError> fallback) : base(fallback)
    {
        _customerErrorResponse1Value = customerErrorResponse1Value;
        _noContentValue = noContentValue;
    }

    private static UpdateCustomerError AsCustomerErrorResponse1(CustomerErrorResponse1 value) =>
        new(Optional<CustomerErrorResponse1>.Some(value), default, default);

    private static UpdateCustomerError AsNoContent(RawError value) =>
        new(default, Optional<RawError>.Some(value), default);

    private static UpdateCustomerError AsFallback(RawError value) =>
        new(default, default, Optional<RawError>.Some(value));

    public bool TryGetCustomerErrorResponse1([NotNullWhen(true)] out CustomerErrorResponse1? value) =>
        _customerErrorResponse1Value.TryGetValue(out value);

    public bool TryGetNoContent([NotNullWhen(true)] out RawError? value) => _noContentValue.TryGetValue(out value);

    public TResult Match<TResult>(Func<CustomerErrorResponse1, TResult> onCustomerErrorResponse1,
        Func<RawError, TResult> onNoContent,
        Func<RawError, TResult> otherwise) =>
        this switch
        {
            _ when TryGetCustomerErrorResponse1(out var customerErrorResponse1Value) => onCustomerErrorResponse1(
                customerErrorResponse1Value),
            _ when TryGetNoContent(out var noContentValue) => onNoContent(noContentValue),
            _ when TryGetRawError(out var fallback) => otherwise(fallback),
            _ => throw new InvalidOperationException($"{nameof(UpdateCustomerError)} holds no value.")
        };

    public void Match(Action<CustomerErrorResponse1> onCustomerErrorResponse1,
        Action<RawError> onNoContent,
        Action<RawError> otherwise)
    {
        if (TryGetCustomerErrorResponse1(out var customerErrorResponse1Value)) onCustomerErrorResponse1(
            customerErrorResponse1Value);
        else if (TryGetNoContent(out var noContentValue)) onNoContent(noContentValue);
        else if (TryGetRawError(out var fallback)) otherwise(fallback);
        else throw new InvalidOperationException($"{nameof(UpdateCustomerError)} holds no value.");
    }

    private static Task<UpdateCustomerError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            404 => response.RawBody().As(AsNoContent),
            422 => response.Json<CustomerErrorResponse1>().As(AsCustomerErrorResponse1),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<UpdateCustomerError> Response { get; } = new(Create);
}
