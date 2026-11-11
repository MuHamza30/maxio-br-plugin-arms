using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class OverrideSubscriptionError : ApiError
{
    private readonly Optional<RawError> _noContentValue;

    private readonly Optional<SingleErrorResponse1> _singleErrorResponse1Value;

    private OverrideSubscriptionError(Optional<RawError> noContentValue,
        Optional<SingleErrorResponse1> singleErrorResponse1Value,
        Optional<RawError> fallback) : base(fallback)
    {
        _noContentValue = noContentValue;
        _singleErrorResponse1Value = singleErrorResponse1Value;
    }

    private static OverrideSubscriptionError AsNoContent(RawError value) =>
        new(Optional<RawError>.Some(value), default, default);

    private static OverrideSubscriptionError AsSingleErrorResponse1(SingleErrorResponse1 value) =>
        new(default, Optional<SingleErrorResponse1>.Some(value), default);

    private static OverrideSubscriptionError AsFallback(RawError value) =>
        new(default, default, Optional<RawError>.Some(value));

    public bool TryGetNoContent(out RawError value) => _noContentValue.TryGetValue(out value);

    public bool TryGetSingleErrorResponse1(out SingleErrorResponse1 value) =>
        _singleErrorResponse1Value.TryGetValue(out value);

    private static Task<OverrideSubscriptionError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            400 => response.RawBody().As(AsNoContent),
            422 => response.Json<SingleErrorResponse1>().As(AsSingleErrorResponse1),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<OverrideSubscriptionError> Response { get; } = new(Create);
}
