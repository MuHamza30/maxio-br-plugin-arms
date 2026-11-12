using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class ListSegmentsForPricePointError : ApiError
{
    private readonly Optional<EventBasedBillingListSegmentsErrors1> _eventBasedBillingListSegmentsErrors1Value;

    private readonly Optional<RawError> _noContentValue;

    private ListSegmentsForPricePointError(Optional<EventBasedBillingListSegmentsErrors1> eventBasedBillingListSegmentsErrors1Value,
        Optional<RawError> noContentValue,
        Optional<RawError> fallback) : base(fallback)
    {
        _eventBasedBillingListSegmentsErrors1Value = eventBasedBillingListSegmentsErrors1Value;
        _noContentValue = noContentValue;
    }

    private static ListSegmentsForPricePointError AsEventBasedBillingListSegmentsErrors1(EventBasedBillingListSegmentsErrors1 value) =>
        new(Optional<EventBasedBillingListSegmentsErrors1>.Some(value), default, default);

    private static ListSegmentsForPricePointError AsNoContent(RawError value) =>
        new(default, Optional<RawError>.Some(value), default);

    private static ListSegmentsForPricePointError AsFallback(RawError value) =>
        new(default, default, Optional<RawError>.Some(value));

    public bool TryGetEventBasedBillingListSegmentsErrors1([NotNullWhen(true)] out EventBasedBillingListSegmentsErrors1? value) =>
        _eventBasedBillingListSegmentsErrors1Value.TryGetValue(out value);

    public bool TryGetNoContent([NotNullWhen(true)] out RawError? value) => _noContentValue.TryGetValue(out value);

    public TResult Match<TResult>(Func<EventBasedBillingListSegmentsErrors1, TResult> onEventBasedBillingListSegmentsErrors1,
        Func<RawError, TResult> onNoContent,
        Func<RawError, TResult> otherwise) =>
        this switch
        {
            _ when TryGetEventBasedBillingListSegmentsErrors1(
                out var eventBasedBillingListSegmentsErrors1Value) => onEventBasedBillingListSegmentsErrors1(
                eventBasedBillingListSegmentsErrors1Value),
            _ when TryGetNoContent(out var noContentValue) => onNoContent(noContentValue),
            _ when TryGetRawError(out var fallback) => otherwise(fallback),
            _ => throw new InvalidOperationException($"{nameof(ListSegmentsForPricePointError)} holds no value.")
        };

    public void Match(Action<EventBasedBillingListSegmentsErrors1> onEventBasedBillingListSegmentsErrors1,
        Action<RawError> onNoContent,
        Action<RawError> otherwise)
    {
        if (TryGetEventBasedBillingListSegmentsErrors1(
            out var eventBasedBillingListSegmentsErrors1Value)) onEventBasedBillingListSegmentsErrors1(
            eventBasedBillingListSegmentsErrors1Value);
        else if (TryGetNoContent(out var noContentValue)) onNoContent(noContentValue);
        else if (TryGetRawError(out var fallback)) otherwise(fallback);
        else throw new InvalidOperationException($"{nameof(ListSegmentsForPricePointError)} holds no value.");
    }

    private static Task<ListSegmentsForPricePointError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            404 => response.RawBody().As(AsNoContent),
            422 => response.Json<EventBasedBillingListSegmentsErrors1>().As(AsEventBasedBillingListSegmentsErrors1),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<ListSegmentsForPricePointError> Response { get; } = new(Create);
}
