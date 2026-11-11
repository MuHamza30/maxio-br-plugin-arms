using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class DeleteReasonCodeError : ApiError
{
    private readonly Optional<RawError> _noContentValue;

    private DeleteReasonCodeError(Optional<RawError> noContentValue, Optional<RawError> fallback) : base(fallback)
    {
        _noContentValue = noContentValue;
    }

    private static DeleteReasonCodeError AsNoContent(RawError value) => new(Optional<RawError>.Some(value), default);

    private static DeleteReasonCodeError AsFallback(RawError value) => new(default, Optional<RawError>.Some(value));

    public bool TryGetNoContent(out RawError value) => _noContentValue.TryGetValue(out value);

    private static Task<DeleteReasonCodeError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            404 => response.RawBody().As(AsNoContent),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<DeleteReasonCodeError> Response { get; } = new(Create);
}
