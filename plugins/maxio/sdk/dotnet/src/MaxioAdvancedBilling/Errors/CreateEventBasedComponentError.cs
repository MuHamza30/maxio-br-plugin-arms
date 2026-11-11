using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class CreateEventBasedComponentError : ApiError
{
    private readonly Optional<RawError> _noContentValue;

    private readonly Optional<ErrorListResponse1> _errorListResponse1Value;

    private CreateEventBasedComponentError(Optional<RawError> noContentValue,
        Optional<ErrorListResponse1> errorListResponse1Value,
        Optional<RawError> fallback) : base(fallback)
    {
        _noContentValue = noContentValue;
        _errorListResponse1Value = errorListResponse1Value;
    }

    private static CreateEventBasedComponentError AsNoContent(RawError value) =>
        new(Optional<RawError>.Some(value), default, default);

    private static CreateEventBasedComponentError AsErrorListResponse1(ErrorListResponse1 value) =>
        new(default, Optional<ErrorListResponse1>.Some(value), default);

    private static CreateEventBasedComponentError AsFallback(RawError value) =>
        new(default, default, Optional<RawError>.Some(value));

    public bool TryGetNoContent(out RawError value) => _noContentValue.TryGetValue(out value);

    public bool TryGetErrorListResponse1(out ErrorListResponse1 value) =>
        _errorListResponse1Value.TryGetValue(out value);

    private static Task<CreateEventBasedComponentError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            404 => response.RawBody().As(AsNoContent),
            422 => response.Json<ErrorListResponse1>().As(AsErrorListResponse1),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<CreateEventBasedComponentError> Response { get; } = new(Create);
}
