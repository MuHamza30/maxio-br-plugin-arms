using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class ListExportedInvoicesError : ApiError
{
    private readonly Optional<RawError> _noContentValue;

    private ListExportedInvoicesError(Optional<RawError> noContentValue, Optional<RawError> fallback) : base(fallback)
    {
        _noContentValue = noContentValue;
    }

    private static ListExportedInvoicesError AsNoContent(RawError value) =>
        new(Optional<RawError>.Some(value), default);

    private static ListExportedInvoicesError AsFallback(RawError value) => new(default, Optional<RawError>.Some(value));

    public bool TryGetNoContent(out RawError value) => _noContentValue.TryGetValue(out value);

    private static Task<ListExportedInvoicesError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            404 => response.RawBody().As(AsNoContent),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<ListExportedInvoicesError> Response { get; } = new(Create);
}
