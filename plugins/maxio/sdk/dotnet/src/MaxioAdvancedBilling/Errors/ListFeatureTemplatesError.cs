using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class ListFeatureTemplatesError : ApiError
{
    private readonly Optional<ErrorListResponse1> _errorListResponse1Value;

    private ListFeatureTemplatesError(Optional<ErrorListResponse1> errorListResponse1Value,
        Optional<RawError> fallback) : base(fallback)
    {
        _errorListResponse1Value = errorListResponse1Value;
    }

    private static ListFeatureTemplatesError AsErrorListResponse1(ErrorListResponse1 value) =>
        new(Optional<ErrorListResponse1>.Some(value), default);

    private static ListFeatureTemplatesError AsFallback(RawError value) => new(default, Optional<RawError>.Some(value));

    public bool TryGetErrorListResponse1(out ErrorListResponse1 value) =>
        _errorListResponse1Value.TryGetValue(out value);

    private static Task<ListFeatureTemplatesError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            403 => response.Json<ErrorListResponse1>().As(AsErrorListResponse1),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<ListFeatureTemplatesError> Response { get; } = new(Create);
}
