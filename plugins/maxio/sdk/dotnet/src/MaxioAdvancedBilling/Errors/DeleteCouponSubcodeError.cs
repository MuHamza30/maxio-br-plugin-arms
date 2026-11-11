using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class DeleteCouponSubcodeError : ApiError
{
    private readonly Optional<RawError> _noContentValue;

    private DeleteCouponSubcodeError(Optional<RawError> noContentValue, Optional<RawError> fallback) : base(fallback)
    {
        _noContentValue = noContentValue;
    }

    private static DeleteCouponSubcodeError AsNoContent(RawError value) => new(Optional<RawError>.Some(value), default);

    private static DeleteCouponSubcodeError AsFallback(RawError value) => new(default, Optional<RawError>.Some(value));

    public bool TryGetNoContent(out RawError value) => _noContentValue.TryGetValue(out value);

    private static Task<DeleteCouponSubcodeError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            404 => response.RawBody().As(AsNoContent),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<DeleteCouponSubcodeError> Response { get; } = new(Create);
}
