using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class BulkUpdateSubscriptionComponentsPricePointsError : ApiError
{
    private readonly Optional<ComponentPricePointError1> _componentPricePointError1Value;

    private BulkUpdateSubscriptionComponentsPricePointsError(Optional<ComponentPricePointError1> componentPricePointError1Value,
        Optional<RawError> fallback) : base(fallback)
    {
        _componentPricePointError1Value = componentPricePointError1Value;
    }

    private static BulkUpdateSubscriptionComponentsPricePointsError AsComponentPricePointError1(ComponentPricePointError1 value) =>
        new(Optional<ComponentPricePointError1>.Some(value), default);

    private static BulkUpdateSubscriptionComponentsPricePointsError AsFallback(RawError value) =>
        new(default, Optional<RawError>.Some(value));

    public bool TryGetComponentPricePointError1(out ComponentPricePointError1 value) =>
        _componentPricePointError1Value.TryGetValue(out value);

    private static Task<BulkUpdateSubscriptionComponentsPricePointsError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            422 => response.Json<ComponentPricePointError1>().As(AsComponentPricePointError1),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<BulkUpdateSubscriptionComponentsPricePointsError> Response { get; } = new(Create);
}
