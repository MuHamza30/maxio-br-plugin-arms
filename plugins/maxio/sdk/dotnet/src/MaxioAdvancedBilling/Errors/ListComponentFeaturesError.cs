using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class ListComponentFeaturesError : ApiError
{
    private readonly Optional<ErrorListResponse1> _errorListResponse1Value;

    private readonly Optional<RawError> _noContentValue;

    private ListComponentFeaturesError(Optional<ErrorListResponse1> errorListResponse1Value,
        Optional<RawError> noContentValue,
        Optional<RawError> fallback) : base(fallback)
    {
        _errorListResponse1Value = errorListResponse1Value;
        _noContentValue = noContentValue;
    }

    private static ListComponentFeaturesError AsErrorListResponse1(ErrorListResponse1 value) =>
        new(Optional<ErrorListResponse1>.Some(value), default, default);

    private static ListComponentFeaturesError AsNoContent(RawError value) =>
        new(default, Optional<RawError>.Some(value), default);

    private static ListComponentFeaturesError AsFallback(RawError value) =>
        new(default, default, Optional<RawError>.Some(value));

    public bool TryGetErrorListResponse1(out ErrorListResponse1 value) =>
        _errorListResponse1Value.TryGetValue(out value);

    public bool TryGetNoContent(out RawError value) => _noContentValue.TryGetValue(out value);

    private static Task<ListComponentFeaturesError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            403 => response.Json<ErrorListResponse1>().As(AsErrorListResponse1),
            404 => response.RawBody().As(AsNoContent),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<ListComponentFeaturesError> Response { get; } = new(Create);
}
