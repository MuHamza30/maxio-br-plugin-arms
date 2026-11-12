using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class CreateCustomerError : ApiError
{
    private readonly Optional<CustomerErrorResponse1> _customerErrorResponse1Value;

    private CreateCustomerError(Optional<CustomerErrorResponse1> customerErrorResponse1Value,
        Optional<RawError> fallback) : base(fallback)
    {
        _customerErrorResponse1Value = customerErrorResponse1Value;
    }

    private static CreateCustomerError AsCustomerErrorResponse1(CustomerErrorResponse1 value) =>
        new(Optional<CustomerErrorResponse1>.Some(value), default);

    private static CreateCustomerError AsFallback(RawError value) => new(default, Optional<RawError>.Some(value));

    public bool TryGetCustomerErrorResponse1([NotNullWhen(true)] out CustomerErrorResponse1? value) =>
        _customerErrorResponse1Value.TryGetValue(out value);

    public TResult Match<TResult>(Func<CustomerErrorResponse1, TResult> onCustomerErrorResponse1,
        Func<RawError, TResult> otherwise) =>
        this switch
        {
            _ when TryGetCustomerErrorResponse1(out var customerErrorResponse1Value) => onCustomerErrorResponse1(
                customerErrorResponse1Value),
            _ when TryGetRawError(out var fallback) => otherwise(fallback),
            _ => throw new InvalidOperationException($"{nameof(CreateCustomerError)} holds no value.")
        };

    public void Match(Action<CustomerErrorResponse1> onCustomerErrorResponse1, Action<RawError> otherwise)
    {
        if (TryGetCustomerErrorResponse1(out var customerErrorResponse1Value)) onCustomerErrorResponse1(
            customerErrorResponse1Value);
        else if (TryGetRawError(out var fallback)) otherwise(fallback);
        else throw new InvalidOperationException($"{nameof(CreateCustomerError)} holds no value.");
    }

    private static Task<CreateCustomerError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            422 => response.Json<CustomerErrorResponse1>().As(AsCustomerErrorResponse1),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<CreateCustomerError> Response { get; } = new(Create);
}
