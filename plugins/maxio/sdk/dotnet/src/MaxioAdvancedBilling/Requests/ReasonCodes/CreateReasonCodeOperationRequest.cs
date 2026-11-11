using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.ReasonCodes;

/// <summary>
/// The inputs of the CreateReasonCode operation.
/// </summary>
public sealed record CreateReasonCodeOperationRequest
{
    public CreateReasonCodeRequest? Body { get; init; }
}
