using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models.AnyOf;

namespace MaxioAdvancedBilling.Errors;

public sealed class CreatePrepaymentError : ApiError
{
    private readonly Optional<CreatePrepaymentErrorResponse> _createPrepaymentErrorResponseValue;

    private CreatePrepaymentError(Optional<CreatePrepaymentErrorResponse> createPrepaymentErrorResponseValue,
        Optional<RawError> fallback) : base(fallback)
    {
        _createPrepaymentErrorResponseValue = createPrepaymentErrorResponseValue;
    }

    private static CreatePrepaymentError AsCreatePrepaymentErrorResponse(CreatePrepaymentErrorResponse value) =>
        new(Optional<CreatePrepaymentErrorResponse>.Some(value), default);

    private static CreatePrepaymentError AsFallback(RawError value) => new(default, Optional<RawError>.Some(value));

    public bool TryGetCreatePrepaymentErrorResponse(out CreatePrepaymentErrorResponse value) =>
        _createPrepaymentErrorResponseValue.TryGetValue(out value);

    private static Task<CreatePrepaymentError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            422 => response.Json<CreatePrepaymentErrorResponse>().As(AsCreatePrepaymentErrorResponse),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<CreatePrepaymentError> Response { get; } = new(Create);
}
