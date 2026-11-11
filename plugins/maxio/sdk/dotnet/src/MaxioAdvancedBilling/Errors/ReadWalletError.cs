using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class ReadWalletError : ApiError
{
    private readonly Optional<RawError> _noContentValue;

    private readonly Optional<WalletErrorResponse1> _walletErrorResponse1Value;

    private ReadWalletError(Optional<RawError> noContentValue,
        Optional<WalletErrorResponse1> walletErrorResponse1Value,
        Optional<RawError> fallback) : base(fallback)
    {
        _noContentValue = noContentValue;
        _walletErrorResponse1Value = walletErrorResponse1Value;
    }

    private static ReadWalletError AsNoContent(RawError value) => new(Optional<RawError>.Some(value), default, default);

    private static ReadWalletError AsWalletErrorResponse1(WalletErrorResponse1 value) =>
        new(default, Optional<WalletErrorResponse1>.Some(value), default);

    private static ReadWalletError AsFallback(RawError value) => new(default, default, Optional<RawError>.Some(value));

    public bool TryGetNoContent(out RawError value) => _noContentValue.TryGetValue(out value);

    public bool TryGetWalletErrorResponse1(out WalletErrorResponse1 value) =>
        _walletErrorResponse1Value.TryGetValue(out value);

    private static Task<ReadWalletError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            403 => response.RawBody().As(AsNoContent),
            404 => response.Json<WalletErrorResponse1>().As(AsWalletErrorResponse1),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<ReadWalletError> Response { get; } = new(Create);
}
