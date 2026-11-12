using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class CreateSubscriptionGroupError : ApiError
{
    private readonly Optional<SubscriptionGroupCreateErrorResponse1> _subscriptionGroupCreateErrorResponse1Value;

    private CreateSubscriptionGroupError(Optional<SubscriptionGroupCreateErrorResponse1> subscriptionGroupCreateErrorResponse1Value,
        Optional<RawError> fallback) : base(fallback)
    {
        _subscriptionGroupCreateErrorResponse1Value = subscriptionGroupCreateErrorResponse1Value;
    }

    private static CreateSubscriptionGroupError AsSubscriptionGroupCreateErrorResponse1(SubscriptionGroupCreateErrorResponse1 value) =>
        new(Optional<SubscriptionGroupCreateErrorResponse1>.Some(value), default);

    private static CreateSubscriptionGroupError AsFallback(RawError value) =>
        new(default, Optional<RawError>.Some(value));

    public bool TryGetSubscriptionGroupCreateErrorResponse1([NotNullWhen(true)] out SubscriptionGroupCreateErrorResponse1? value) =>
        _subscriptionGroupCreateErrorResponse1Value.TryGetValue(out value);

    public TResult Match<TResult>(Func<SubscriptionGroupCreateErrorResponse1, TResult> onSubscriptionGroupCreateErrorResponse1,
        Func<RawError, TResult> otherwise) =>
        this switch
        {
            _ when TryGetSubscriptionGroupCreateErrorResponse1(
                out var subscriptionGroupCreateErrorResponse1Value) => onSubscriptionGroupCreateErrorResponse1(
                subscriptionGroupCreateErrorResponse1Value),
            _ when TryGetRawError(out var fallback) => otherwise(fallback),
            _ => throw new InvalidOperationException($"{nameof(CreateSubscriptionGroupError)} holds no value.")
        };

    public void Match(Action<SubscriptionGroupCreateErrorResponse1> onSubscriptionGroupCreateErrorResponse1,
        Action<RawError> otherwise)
    {
        if (TryGetSubscriptionGroupCreateErrorResponse1(
            out var subscriptionGroupCreateErrorResponse1Value)) onSubscriptionGroupCreateErrorResponse1(
            subscriptionGroupCreateErrorResponse1Value);
        else if (TryGetRawError(out var fallback)) otherwise(fallback);
        else throw new InvalidOperationException($"{nameof(CreateSubscriptionGroupError)} holds no value.");
    }

    private static Task<CreateSubscriptionGroupError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            422 => response.Json<SubscriptionGroupCreateErrorResponse1>().As(AsSubscriptionGroupCreateErrorResponse1),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<CreateSubscriptionGroupError> Response { get; } = new(Create);
}
