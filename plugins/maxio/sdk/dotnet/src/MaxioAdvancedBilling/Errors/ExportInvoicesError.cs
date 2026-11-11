using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class ExportInvoicesError : ApiError
{
    private readonly Optional<RawError> _noContentValue;

    private readonly Optional<SingleErrorResponse1> _singleErrorResponse1Value;

    private ExportInvoicesError(Optional<RawError> noContentValue,
        Optional<SingleErrorResponse1> singleErrorResponse1Value,
        Optional<RawError> fallback) : base(fallback)
    {
        _noContentValue = noContentValue;
        _singleErrorResponse1Value = singleErrorResponse1Value;
    }

    private static ExportInvoicesError AsNoContent(RawError value) =>
        new(Optional<RawError>.Some(value), default, default);

    private static ExportInvoicesError AsSingleErrorResponse1(SingleErrorResponse1 value) =>
        new(default, Optional<SingleErrorResponse1>.Some(value), default);

    private static ExportInvoicesError AsFallback(RawError value) =>
        new(default, default, Optional<RawError>.Some(value));

    public bool TryGetNoContent(out RawError value) => _noContentValue.TryGetValue(out value);

    public bool TryGetSingleErrorResponse1(out SingleErrorResponse1 value) =>
        _singleErrorResponse1Value.TryGetValue(out value);

    private static Task<ExportInvoicesError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            404 => response.RawBody().As(AsNoContent),
            409 => response.Json<SingleErrorResponse1>().As(AsSingleErrorResponse1),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<ExportInvoicesError> Response { get; } = new(Create);
}
