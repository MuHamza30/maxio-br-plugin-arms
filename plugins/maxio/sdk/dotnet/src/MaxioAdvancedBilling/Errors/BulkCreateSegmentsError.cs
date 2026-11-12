using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class BulkCreateSegmentsError : ApiError
{
    private readonly Optional<EventBasedBillingSegment1> _eventBasedBillingSegment1Value;

    private readonly Optional<RawError> _noContentValue;

    private BulkCreateSegmentsError(Optional<EventBasedBillingSegment1> eventBasedBillingSegment1Value,
        Optional<RawError> noContentValue,
        Optional<RawError> fallback) : base(fallback)
    {
        _eventBasedBillingSegment1Value = eventBasedBillingSegment1Value;
        _noContentValue = noContentValue;
    }

    private static BulkCreateSegmentsError AsEventBasedBillingSegment1(EventBasedBillingSegment1 value) =>
        new(Optional<EventBasedBillingSegment1>.Some(value), default, default);

    private static BulkCreateSegmentsError AsNoContent(RawError value) =>
        new(default, Optional<RawError>.Some(value), default);

    private static BulkCreateSegmentsError AsFallback(RawError value) =>
        new(default, default, Optional<RawError>.Some(value));

    public bool TryGetEventBasedBillingSegment1([NotNullWhen(true)] out EventBasedBillingSegment1? value) =>
        _eventBasedBillingSegment1Value.TryGetValue(out value);

    public bool TryGetNoContent([NotNullWhen(true)] out RawError? value) => _noContentValue.TryGetValue(out value);

    public TResult Match<TResult>(Func<EventBasedBillingSegment1, TResult> onEventBasedBillingSegment1,
        Func<RawError, TResult> onNoContent,
        Func<RawError, TResult> otherwise) =>
        this switch
        {
            _ when TryGetEventBasedBillingSegment1(
                out var eventBasedBillingSegment1Value) => onEventBasedBillingSegment1(eventBasedBillingSegment1Value),
            _ when TryGetNoContent(out var noContentValue) => onNoContent(noContentValue),
            _ when TryGetRawError(out var fallback) => otherwise(fallback),
            _ => throw new InvalidOperationException($"{nameof(BulkCreateSegmentsError)} holds no value.")
        };

    public void Match(Action<EventBasedBillingSegment1> onEventBasedBillingSegment1,
        Action<RawError> onNoContent,
        Action<RawError> otherwise)
    {
        if (TryGetEventBasedBillingSegment1(out var eventBasedBillingSegment1Value)) onEventBasedBillingSegment1(
            eventBasedBillingSegment1Value);
        else if (TryGetNoContent(out var noContentValue)) onNoContent(noContentValue);
        else if (TryGetRawError(out var fallback)) otherwise(fallback);
        else throw new InvalidOperationException($"{nameof(BulkCreateSegmentsError)} holds no value.");
    }

    private static Task<BulkCreateSegmentsError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            404 => response.RawBody().As(AsNoContent),
            422 => response.Json<EventBasedBillingSegment1>().As(AsEventBasedBillingSegment1),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<BulkCreateSegmentsError> Response { get; } = new(Create);
}
