using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class BulkRecordEventsError : ApiError
{
    private readonly Optional<RawError> _noContentValue;

    private BulkRecordEventsError(Optional<RawError> noContentValue, Optional<RawError> fallback) : base(fallback)
    {
        _noContentValue = noContentValue;
    }

    private static BulkRecordEventsError AsNoContent(RawError value) => new(Optional<RawError>.Some(value), default);

    private static BulkRecordEventsError AsFallback(RawError value) => new(default, Optional<RawError>.Some(value));

    public bool TryGetNoContent(out RawError value) => _noContentValue.TryGetValue(out value);

    private static Task<BulkRecordEventsError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            422 => response.RawBody().As(AsNoContent),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<BulkRecordEventsError> Response { get; } = new(Create);
}
