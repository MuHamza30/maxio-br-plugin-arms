using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models.AnyOf;

namespace MaxioAdvancedBilling.Errors;

public sealed class UpdatePrepaidSubscriptionConfigurationError : ApiError
{
    private readonly Optional<PrepaidConfigurationErrorResponse> _prepaidConfigurationErrorResponseValue;

    private UpdatePrepaidSubscriptionConfigurationError(Optional<PrepaidConfigurationErrorResponse> prepaidConfigurationErrorResponseValue,
        Optional<RawError> fallback) : base(fallback)
    {
        _prepaidConfigurationErrorResponseValue = prepaidConfigurationErrorResponseValue;
    }

    private static UpdatePrepaidSubscriptionConfigurationError AsPrepaidConfigurationErrorResponse(PrepaidConfigurationErrorResponse value) =>
        new(Optional<PrepaidConfigurationErrorResponse>.Some(value), default);

    private static UpdatePrepaidSubscriptionConfigurationError AsFallback(RawError value) =>
        new(default, Optional<RawError>.Some(value));

    public bool TryGetPrepaidConfigurationErrorResponse(out PrepaidConfigurationErrorResponse value) =>
        _prepaidConfigurationErrorResponseValue.TryGetValue(out value);

    private static Task<UpdatePrepaidSubscriptionConfigurationError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            422 => response.Json<PrepaidConfigurationErrorResponse>().As(AsPrepaidConfigurationErrorResponse),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<UpdatePrepaidSubscriptionConfigurationError> Response { get; } = new(Create);
}
