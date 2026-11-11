using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class ReadInvoicesExportError : ApiError
{
    private readonly Optional<RawError> _noContentValue;

    private ReadInvoicesExportError(Optional<RawError> noContentValue, Optional<RawError> fallback) : base(fallback)
    {
        _noContentValue = noContentValue;
    }

    private static ReadInvoicesExportError AsNoContent(RawError value) => new(Optional<RawError>.Some(value), default);

    private static ReadInvoicesExportError AsFallback(RawError value) => new(default, Optional<RawError>.Some(value));

    public bool TryGetNoContent(out RawError value) => _noContentValue.TryGetValue(out value);

    private static Task<ReadInvoicesExportError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            404 => response.RawBody().As(AsNoContent),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<ReadInvoicesExportError> Response { get; } = new(Create);
}
