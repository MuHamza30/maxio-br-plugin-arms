using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class UpdateSegmentError : ApiError
{
    private readonly Optional<RawError> _noContentValue;

    private readonly Optional<EventBasedBillingSegmentErrors1> _eventBasedBillingSegmentErrors1Value;

    private UpdateSegmentError(Optional<RawError> noContentValue,
        Optional<EventBasedBillingSegmentErrors1> eventBasedBillingSegmentErrors1Value,
        Optional<RawError> fallback) : base(fallback)
    {
        _noContentValue = noContentValue;
        _eventBasedBillingSegmentErrors1Value = eventBasedBillingSegmentErrors1Value;
    }

    private static UpdateSegmentError AsNoContent(RawError value) =>
        new(Optional<RawError>.Some(value), default, default);

    private static UpdateSegmentError AsEventBasedBillingSegmentErrors1(EventBasedBillingSegmentErrors1 value) =>
        new(default, Optional<EventBasedBillingSegmentErrors1>.Some(value), default);

    private static UpdateSegmentError AsFallback(RawError value) =>
        new(default, default, Optional<RawError>.Some(value));

    public bool TryGetNoContent(out RawError value) => _noContentValue.TryGetValue(out value);

    public bool TryGetEventBasedBillingSegmentErrors1(out EventBasedBillingSegmentErrors1 value) =>
        _eventBasedBillingSegmentErrors1Value.TryGetValue(out value);

    private static Task<UpdateSegmentError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            404 => response.RawBody().As(AsNoContent),
            422 => response.Json<EventBasedBillingSegmentErrors1>().As(AsEventBasedBillingSegmentErrors1),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<UpdateSegmentError> Response { get; } = new(Create);
}
