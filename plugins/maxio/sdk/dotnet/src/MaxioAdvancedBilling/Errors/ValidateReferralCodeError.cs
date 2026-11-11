using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class ValidateReferralCodeError : ApiError
{
    private readonly Optional<SingleStringErrorResponse1> _singleStringErrorResponse1Value;

    private ValidateReferralCodeError(Optional<SingleStringErrorResponse1> singleStringErrorResponse1Value,
        Optional<RawError> fallback) : base(fallback)
    {
        _singleStringErrorResponse1Value = singleStringErrorResponse1Value;
    }

    private static ValidateReferralCodeError AsSingleStringErrorResponse1(SingleStringErrorResponse1 value) =>
        new(Optional<SingleStringErrorResponse1>.Some(value), default);

    private static ValidateReferralCodeError AsFallback(RawError value) => new(default, Optional<RawError>.Some(value));

    public bool TryGetSingleStringErrorResponse1(out SingleStringErrorResponse1 value) =>
        _singleStringErrorResponse1Value.TryGetValue(out value);

    private static Task<ValidateReferralCodeError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            404 => response.Json<SingleStringErrorResponse1>().As(AsSingleStringErrorResponse1),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<ValidateReferralCodeError> Response { get; } = new(Create);
}
