using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class ListSegmentsForPricePointError : ApiError
{
    private readonly Optional<RawError> _noContentValue;

    private readonly Optional<EventBasedBillingListSegmentsErrors1> _eventBasedBillingListSegmentsErrors1Value;

    private ListSegmentsForPricePointError(Optional<RawError> noContentValue,
        Optional<EventBasedBillingListSegmentsErrors1> eventBasedBillingListSegmentsErrors1Value,
        Optional<RawError> fallback) : base(fallback)
    {
        _noContentValue = noContentValue;
        _eventBasedBillingListSegmentsErrors1Value = eventBasedBillingListSegmentsErrors1Value;
    }

    private static ListSegmentsForPricePointError AsNoContent(RawError value) =>
        new(Optional<RawError>.Some(value), default, default);

    private static ListSegmentsForPricePointError AsEventBasedBillingListSegmentsErrors1(EventBasedBillingListSegmentsErrors1 value) =>
        new(default, Optional<EventBasedBillingListSegmentsErrors1>.Some(value), default);

    private static ListSegmentsForPricePointError AsFallback(RawError value) =>
        new(default, default, Optional<RawError>.Some(value));

    public bool TryGetNoContent(out RawError value) => _noContentValue.TryGetValue(out value);

    public bool TryGetEventBasedBillingListSegmentsErrors1(out EventBasedBillingListSegmentsErrors1 value) =>
        _eventBasedBillingListSegmentsErrors1Value.TryGetValue(out value);

    private static Task<ListSegmentsForPricePointError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            404 => response.RawBody().As(AsNoContent),
            422 => response.Json<EventBasedBillingListSegmentsErrors1>().As(AsEventBasedBillingListSegmentsErrors1),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<ListSegmentsForPricePointError> Response { get; } = new(Create);
}
