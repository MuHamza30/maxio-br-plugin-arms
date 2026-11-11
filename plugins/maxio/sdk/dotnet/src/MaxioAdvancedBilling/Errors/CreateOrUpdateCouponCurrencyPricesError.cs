using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class CreateOrUpdateCouponCurrencyPricesError : ApiError
{
    private readonly Optional<ErrorStringMapResponse1> _errorStringMapResponse1Value;

    private CreateOrUpdateCouponCurrencyPricesError(Optional<ErrorStringMapResponse1> errorStringMapResponse1Value,
        Optional<RawError> fallback) : base(fallback)
    {
        _errorStringMapResponse1Value = errorStringMapResponse1Value;
    }

    private static CreateOrUpdateCouponCurrencyPricesError AsErrorStringMapResponse1(ErrorStringMapResponse1 value) =>
        new(Optional<ErrorStringMapResponse1>.Some(value), default);

    private static CreateOrUpdateCouponCurrencyPricesError AsFallback(RawError value) =>
        new(default, Optional<RawError>.Some(value));

    public bool TryGetErrorStringMapResponse1(out ErrorStringMapResponse1 value) =>
        _errorStringMapResponse1Value.TryGetValue(out value);

    private static Task<CreateOrUpdateCouponCurrencyPricesError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            422 => response.Json<ErrorStringMapResponse1>().As(AsErrorStringMapResponse1),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<CreateOrUpdateCouponCurrencyPricesError> Response { get; } = new(Create);
}
