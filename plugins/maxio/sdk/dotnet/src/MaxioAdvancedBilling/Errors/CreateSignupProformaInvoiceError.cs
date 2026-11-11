using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class CreateSignupProformaInvoiceError : ApiError
{
    private readonly Optional<ProformaBadRequestErrorResponse1> _proformaBadRequestErrorResponse1Value;

    private readonly Optional<ErrorArrayMapResponse1> _errorArrayMapResponse1Value;

    private CreateSignupProformaInvoiceError(Optional<ProformaBadRequestErrorResponse1> proformaBadRequestErrorResponse1Value,
        Optional<ErrorArrayMapResponse1> errorArrayMapResponse1Value,
        Optional<RawError> fallback) : base(fallback)
    {
        _proformaBadRequestErrorResponse1Value = proformaBadRequestErrorResponse1Value;
        _errorArrayMapResponse1Value = errorArrayMapResponse1Value;
    }

    private static CreateSignupProformaInvoiceError AsProformaBadRequestErrorResponse1(ProformaBadRequestErrorResponse1 value) =>
        new(Optional<ProformaBadRequestErrorResponse1>.Some(value), default, default);

    private static CreateSignupProformaInvoiceError AsErrorArrayMapResponse1(ErrorArrayMapResponse1 value) =>
        new(default, Optional<ErrorArrayMapResponse1>.Some(value), default);

    private static CreateSignupProformaInvoiceError AsFallback(RawError value) =>
        new(default, default, Optional<RawError>.Some(value));

    public bool TryGetProformaBadRequestErrorResponse1(out ProformaBadRequestErrorResponse1 value) =>
        _proformaBadRequestErrorResponse1Value.TryGetValue(out value);

    public bool TryGetErrorArrayMapResponse1(out ErrorArrayMapResponse1 value) =>
        _errorArrayMapResponse1Value.TryGetValue(out value);

    private static Task<CreateSignupProformaInvoiceError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            400 => response.Json<ProformaBadRequestErrorResponse1>().As(AsProformaBadRequestErrorResponse1),
            422 => response.Json<ErrorArrayMapResponse1>().As(AsErrorArrayMapResponse1),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<CreateSignupProformaInvoiceError> Response { get; } = new(Create);
}
