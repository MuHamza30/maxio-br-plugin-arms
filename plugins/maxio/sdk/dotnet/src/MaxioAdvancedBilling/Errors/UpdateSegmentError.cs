using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class UpdateSegmentError : ApiError
{
    private readonly Optional<EventBasedBillingSegmentErrors1> _eventBasedBillingSegmentErrors1Value;

    private readonly Optional<RawError> _noContentValue;

    private UpdateSegmentError(Optional<EventBasedBillingSegmentErrors1> eventBasedBillingSegmentErrors1Value,
        Optional<RawError> noContentValue,
        Optional<RawError> fallback) : base(fallback)
    {
        _eventBasedBillingSegmentErrors1Value = eventBasedBillingSegmentErrors1Value;
        _noContentValue = noContentValue;
    }

    private static UpdateSegmentError AsEventBasedBillingSegmentErrors1(EventBasedBillingSegmentErrors1 value) =>
        new(Optional<EventBasedBillingSegmentErrors1>.Some(value), default, default);

    private static UpdateSegmentError AsNoContent(RawError value) =>
        new(default, Optional<RawError>.Some(value), default);

    private static UpdateSegmentError AsFallback(RawError value) =>
        new(default, default, Optional<RawError>.Some(value));

    public bool TryGetEventBasedBillingSegmentErrors1([NotNullWhen(true)] out EventBasedBillingSegmentErrors1? value) =>
        _eventBasedBillingSegmentErrors1Value.TryGetValue(out value);

    public bool TryGetNoContent([NotNullWhen(true)] out RawError? value) => _noContentValue.TryGetValue(out value);

    public TResult Match<TResult>(Func<EventBasedBillingSegmentErrors1, TResult> onEventBasedBillingSegmentErrors1,
        Func<RawError, TResult> onNoContent,
        Func<RawError, TResult> otherwise) =>
        this switch
        {
            _ when TryGetEventBasedBillingSegmentErrors1(
                out var eventBasedBillingSegmentErrors1Value) => onEventBasedBillingSegmentErrors1(
                eventBasedBillingSegmentErrors1Value),
            _ when TryGetNoContent(out var noContentValue) => onNoContent(noContentValue),
            _ when TryGetRawError(out var fallback) => otherwise(fallback),
            _ => throw new InvalidOperationException($"{nameof(UpdateSegmentError)} holds no value.")
        };

    public void Match(Action<EventBasedBillingSegmentErrors1> onEventBasedBillingSegmentErrors1,
        Action<RawError> onNoContent,
        Action<RawError> otherwise)
    {
        if (TryGetEventBasedBillingSegmentErrors1(
            out var eventBasedBillingSegmentErrors1Value)) onEventBasedBillingSegmentErrors1(
            eventBasedBillingSegmentErrors1Value);
        else if (TryGetNoContent(out var noContentValue)) onNoContent(noContentValue);
        else if (TryGetRawError(out var fallback)) otherwise(fallback);
        else throw new InvalidOperationException($"{nameof(UpdateSegmentError)} holds no value.");
    }

    private static Task<UpdateSegmentError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            404 => response.RawBody().As(AsNoContent),
            422 => response.Json<EventBasedBillingSegmentErrors1>().As(AsEventBasedBillingSegmentErrors1),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<UpdateSegmentError> Response { get; } = new(Create);
}
