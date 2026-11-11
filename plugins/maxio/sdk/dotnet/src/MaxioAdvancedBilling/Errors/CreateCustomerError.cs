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

    public bool TryGetCustomerErrorResponse1(out CustomerErrorResponse1 value) =>
        _customerErrorResponse1Value.TryGetValue(out value);

    private static Task<CreateCustomerError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            422 => response.Json<CustomerErrorResponse1>().As(AsCustomerErrorResponse1),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<CreateCustomerError> Response { get; } = new(Create);
}
