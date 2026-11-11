using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class ReopenInvoiceError : ApiError
{
    private readonly Optional<object?> _objectValue;

    private readonly Optional<ErrorListResponse1> _errorListResponse1Value;

    private ReopenInvoiceError(Optional<object?> objectValue,
        Optional<ErrorListResponse1> errorListResponse1Value,
        Optional<RawError> fallback) : base(fallback)
    {
        _objectValue = objectValue;
        _errorListResponse1Value = errorListResponse1Value;
    }

    private static ReopenInvoiceError AsObject(object? value) => new(Optional<object?>.Some(value), default, default);

    private static ReopenInvoiceError AsErrorListResponse1(ErrorListResponse1 value) =>
        new(default, Optional<ErrorListResponse1>.Some(value), default);

    private static ReopenInvoiceError AsFallback(RawError value) =>
        new(default, default, Optional<RawError>.Some(value));

    public bool TryGetObject(out object? value) => _objectValue.TryGetValue(out value);

    public bool TryGetErrorListResponse1(out ErrorListResponse1 value) =>
        _errorListResponse1Value.TryGetValue(out value);

    private static Task<ReopenInvoiceError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            404 => response.Json<object?>().As(AsObject),
            422 => response.Json<ErrorListResponse1>().As(AsErrorListResponse1),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<ReopenInvoiceError> Response { get; } = new(Create);
}
