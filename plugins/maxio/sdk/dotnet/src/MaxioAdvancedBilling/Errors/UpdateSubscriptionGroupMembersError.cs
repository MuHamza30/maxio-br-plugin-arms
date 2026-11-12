using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class UpdateSubscriptionGroupMembersError : ApiError
{
    private readonly Optional<SubscriptionGroupUpdateErrorResponse1> _subscriptionGroupUpdateErrorResponse1Value;

    private UpdateSubscriptionGroupMembersError(Optional<SubscriptionGroupUpdateErrorResponse1> subscriptionGroupUpdateErrorResponse1Value,
        Optional<RawError> fallback) : base(fallback)
    {
        _subscriptionGroupUpdateErrorResponse1Value = subscriptionGroupUpdateErrorResponse1Value;
    }

    private static UpdateSubscriptionGroupMembersError AsSubscriptionGroupUpdateErrorResponse1(SubscriptionGroupUpdateErrorResponse1 value) =>
        new(Optional<SubscriptionGroupUpdateErrorResponse1>.Some(value), default);

    private static UpdateSubscriptionGroupMembersError AsFallback(RawError value) =>
        new(default, Optional<RawError>.Some(value));

    public bool TryGetSubscriptionGroupUpdateErrorResponse1([NotNullWhen(true)] out SubscriptionGroupUpdateErrorResponse1? value) =>
        _subscriptionGroupUpdateErrorResponse1Value.TryGetValue(out value);

    public TResult Match<TResult>(Func<SubscriptionGroupUpdateErrorResponse1, TResult> onSubscriptionGroupUpdateErrorResponse1,
        Func<RawError, TResult> otherwise) =>
        this switch
        {
            _ when TryGetSubscriptionGroupUpdateErrorResponse1(
                out var subscriptionGroupUpdateErrorResponse1Value) => onSubscriptionGroupUpdateErrorResponse1(
                subscriptionGroupUpdateErrorResponse1Value),
            _ when TryGetRawError(out var fallback) => otherwise(fallback),
            _ => throw new InvalidOperationException($"{nameof(UpdateSubscriptionGroupMembersError)} holds no value.")
        };

    public void Match(Action<SubscriptionGroupUpdateErrorResponse1> onSubscriptionGroupUpdateErrorResponse1,
        Action<RawError> otherwise)
    {
        if (TryGetSubscriptionGroupUpdateErrorResponse1(
            out var subscriptionGroupUpdateErrorResponse1Value)) onSubscriptionGroupUpdateErrorResponse1(
            subscriptionGroupUpdateErrorResponse1Value);
        else if (TryGetRawError(out var fallback)) otherwise(fallback);
        else throw new InvalidOperationException($"{nameof(UpdateSubscriptionGroupMembersError)} holds no value.");
    }

    private static Task<UpdateSubscriptionGroupMembersError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            422 => response.Json<SubscriptionGroupUpdateErrorResponse1>().As(AsSubscriptionGroupUpdateErrorResponse1),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<UpdateSubscriptionGroupMembersError> Response { get; } = new(Create);
}
