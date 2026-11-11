using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class DeleteSegmentError : ApiError
{
    private readonly Optional<RawError> _noContentValue;

    private DeleteSegmentError(Optional<RawError> noContentValue, Optional<RawError> fallback) : base(fallback)
    {
        _noContentValue = noContentValue;
    }

    private static DeleteSegmentError AsNoContent(RawError value) => new(Optional<RawError>.Some(value), default);

    private static DeleteSegmentError AsFallback(RawError value) => new(default, Optional<RawError>.Some(value));

    public bool TryGetNoContent(out RawError value) => _noContentValue.TryGetValue(out value);

    private static Task<DeleteSegmentError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            404 or 422 => response.RawBody().As(AsNoContent),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<DeleteSegmentError> Response { get; } = new(Create);
}
