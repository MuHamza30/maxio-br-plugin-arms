using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class PreviewSignupProformaInvoiceError : ApiError
{
    private readonly Optional<ProformaBadRequestErrorResponse1> _proformaBadRequestErrorResponse1Value;

    private readonly Optional<ErrorArrayMapResponse1> _errorArrayMapResponse1Value;

    private PreviewSignupProformaInvoiceError(Optional<ProformaBadRequestErrorResponse1> proformaBadRequestErrorResponse1Value,
        Optional<ErrorArrayMapResponse1> errorArrayMapResponse1Value,
        Optional<RawError> fallback) : base(fallback)
    {
        _proformaBadRequestErrorResponse1Value = proformaBadRequestErrorResponse1Value;
        _errorArrayMapResponse1Value = errorArrayMapResponse1Value;
    }

    private static PreviewSignupProformaInvoiceError AsProformaBadRequestErrorResponse1(ProformaBadRequestErrorResponse1 value) =>
        new(Optional<ProformaBadRequestErrorResponse1>.Some(value), default, default);

    private static PreviewSignupProformaInvoiceError AsErrorArrayMapResponse1(ErrorArrayMapResponse1 value) =>
        new(default, Optional<ErrorArrayMapResponse1>.Some(value), default);

    private static PreviewSignupProformaInvoiceError AsFallback(RawError value) =>
        new(default, default, Optional<RawError>.Some(value));

    public bool TryGetProformaBadRequestErrorResponse1(out ProformaBadRequestErrorResponse1 value) =>
        _proformaBadRequestErrorResponse1Value.TryGetValue(out value);

    public bool TryGetErrorArrayMapResponse1(out ErrorArrayMapResponse1 value) =>
        _errorArrayMapResponse1Value.TryGetValue(out value);

    private static Task<PreviewSignupProformaInvoiceError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            400 => response.Json<ProformaBadRequestErrorResponse1>().As(AsProformaBadRequestErrorResponse1),
            422 => response.Json<ErrorArrayMapResponse1>().As(AsErrorArrayMapResponse1),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<PreviewSignupProformaInvoiceError> Response { get; } = new(Create);
}
