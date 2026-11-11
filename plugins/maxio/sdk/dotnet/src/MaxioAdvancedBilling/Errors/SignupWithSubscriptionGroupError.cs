using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class SignupWithSubscriptionGroupError : ApiError
{
    private readonly Optional<SubscriptionGroupSignupErrorResponse1> _subscriptionGroupSignupErrorResponse1Value;

    private SignupWithSubscriptionGroupError(Optional<SubscriptionGroupSignupErrorResponse1> subscriptionGroupSignupErrorResponse1Value,
        Optional<RawError> fallback) : base(fallback)
    {
        _subscriptionGroupSignupErrorResponse1Value = subscriptionGroupSignupErrorResponse1Value;
    }

    private static SignupWithSubscriptionGroupError AsSubscriptionGroupSignupErrorResponse1(SubscriptionGroupSignupErrorResponse1 value) =>
        new(Optional<SubscriptionGroupSignupErrorResponse1>.Some(value), default);

    private static SignupWithSubscriptionGroupError AsFallback(RawError value) =>
        new(default, Optional<RawError>.Some(value));

    public bool TryGetSubscriptionGroupSignupErrorResponse1(out SubscriptionGroupSignupErrorResponse1 value) =>
        _subscriptionGroupSignupErrorResponse1Value.TryGetValue(out value);

    private static Task<SignupWithSubscriptionGroupError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            422 => response.Json<SubscriptionGroupSignupErrorResponse1>().As(AsSubscriptionGroupSignupErrorResponse1),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<SignupWithSubscriptionGroupError> Response { get; } = new(Create);
}
