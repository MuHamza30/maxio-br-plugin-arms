using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class UpdatePaymentProfileError : ApiError
{
    private readonly Optional<RawError> _noContentValue;

    private readonly Optional<ErrorStringMapResponse1> _errorStringMapResponse1Value;

    private UpdatePaymentProfileError(Optional<RawError> noContentValue,
        Optional<ErrorStringMapResponse1> errorStringMapResponse1Value,
        Optional<RawError> fallback) : base(fallback)
    {
        _noContentValue = noContentValue;
        _errorStringMapResponse1Value = errorStringMapResponse1Value;
    }

    private static UpdatePaymentProfileError AsNoContent(RawError value) =>
        new(Optional<RawError>.Some(value), default, default);

    private static UpdatePaymentProfileError AsErrorStringMapResponse1(ErrorStringMapResponse1 value) =>
        new(default, Optional<ErrorStringMapResponse1>.Some(value), default);

    private static UpdatePaymentProfileError AsFallback(RawError value) =>
        new(default, default, Optional<RawError>.Some(value));

    public bool TryGetNoContent(out RawError value) => _noContentValue.TryGetValue(out value);

    public bool TryGetErrorStringMapResponse1(out ErrorStringMapResponse1 value) =>
        _errorStringMapResponse1Value.TryGetValue(out value);

    private static Task<UpdatePaymentProfileError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            404 => response.RawBody().As(AsNoContent),
            422 => response.Json<ErrorStringMapResponse1>().As(AsErrorStringMapResponse1),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<UpdatePaymentProfileError> Response { get; } = new(Create);
}
