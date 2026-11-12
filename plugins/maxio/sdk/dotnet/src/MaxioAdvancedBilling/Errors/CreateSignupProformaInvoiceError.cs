using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class CreateSignupProformaInvoiceError : ApiError
{
    private readonly Optional<ErrorArrayMapResponse1> _errorArrayMapResponse1Value;

    private readonly Optional<ProformaBadRequestErrorResponse1> _proformaBadRequestErrorResponse1Value;

    private CreateSignupProformaInvoiceError(Optional<ErrorArrayMapResponse1> errorArrayMapResponse1Value,
        Optional<ProformaBadRequestErrorResponse1> proformaBadRequestErrorResponse1Value,
        Optional<RawError> fallback) : base(fallback)
    {
        _errorArrayMapResponse1Value = errorArrayMapResponse1Value;
        _proformaBadRequestErrorResponse1Value = proformaBadRequestErrorResponse1Value;
    }

    private static CreateSignupProformaInvoiceError AsErrorArrayMapResponse1(ErrorArrayMapResponse1 value) =>
        new(Optional<ErrorArrayMapResponse1>.Some(value), default, default);

    private static CreateSignupProformaInvoiceError AsProformaBadRequestErrorResponse1(ProformaBadRequestErrorResponse1 value) =>
        new(default, Optional<ProformaBadRequestErrorResponse1>.Some(value), default);

    private static CreateSignupProformaInvoiceError AsFallback(RawError value) =>
        new(default, default, Optional<RawError>.Some(value));

    public bool TryGetErrorArrayMapResponse1([NotNullWhen(true)] out ErrorArrayMapResponse1? value) =>
        _errorArrayMapResponse1Value.TryGetValue(out value);

    public bool TryGetProformaBadRequestErrorResponse1([NotNullWhen(true)] out ProformaBadRequestErrorResponse1? value) =>
        _proformaBadRequestErrorResponse1Value.TryGetValue(out value);

    public TResult Match<TResult>(Func<ErrorArrayMapResponse1, TResult> onErrorArrayMapResponse1,
        Func<ProformaBadRequestErrorResponse1, TResult> onProformaBadRequestErrorResponse1,
        Func<RawError, TResult> otherwise) =>
        this switch
        {
            _ when TryGetErrorArrayMapResponse1(out var errorArrayMapResponse1Value) => onErrorArrayMapResponse1(
                errorArrayMapResponse1Value),
            _ when TryGetProformaBadRequestErrorResponse1(
                out var proformaBadRequestErrorResponse1Value) => onProformaBadRequestErrorResponse1(
                proformaBadRequestErrorResponse1Value),
            _ when TryGetRawError(out var fallback) => otherwise(fallback),
            _ => throw new InvalidOperationException($"{nameof(CreateSignupProformaInvoiceError)} holds no value.")
        };

    public void Match(Action<ErrorArrayMapResponse1> onErrorArrayMapResponse1,
        Action<ProformaBadRequestErrorResponse1> onProformaBadRequestErrorResponse1,
        Action<RawError> otherwise)
    {
        if (TryGetErrorArrayMapResponse1(out var errorArrayMapResponse1Value)) onErrorArrayMapResponse1(
            errorArrayMapResponse1Value);
        else if (TryGetProformaBadRequestErrorResponse1(
            out var proformaBadRequestErrorResponse1Value)) onProformaBadRequestErrorResponse1(
            proformaBadRequestErrorResponse1Value);
        else if (TryGetRawError(out var fallback)) otherwise(fallback);
        else throw new InvalidOperationException($"{nameof(CreateSignupProformaInvoiceError)} holds no value.");
    }

    private static Task<CreateSignupProformaInvoiceError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            400 => response.Json<ProformaBadRequestErrorResponse1>().As(AsProformaBadRequestErrorResponse1),
            422 => response.Json<ErrorArrayMapResponse1>().As(AsErrorArrayMapResponse1),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<CreateSignupProformaInvoiceError> Response { get; } = new(Create);
}
