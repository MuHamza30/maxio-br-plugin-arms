using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class ListWalletProductsError : ApiError
{
    private readonly Optional<RawError> _noContentValue;

    private ListWalletProductsError(Optional<RawError> noContentValue, Optional<RawError> fallback) : base(fallback)
    {
        _noContentValue = noContentValue;
    }

    private static ListWalletProductsError AsNoContent(RawError value) => new(Optional<RawError>.Some(value), default);

    private static ListWalletProductsError AsFallback(RawError value) => new(default, Optional<RawError>.Some(value));

    public bool TryGetNoContent(out RawError value) => _noContentValue.TryGetValue(out value);

    private static Task<ListWalletProductsError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            403 => response.RawBody().As(AsNoContent),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<ListWalletProductsError> Response { get; } = new(Create);
}
