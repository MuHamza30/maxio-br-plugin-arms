using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.ReasonCodes;

/// <summary>
/// The inputs of the UpdateReasonCode operation.
/// </summary>
public sealed record UpdateReasonCodeOperationRequest
{
    /// <summary>
    /// The Advanced Billing id of the reason code
    /// </summary>
    public required int ReasonCodeId { get; init; }

    public UpdateReasonCodeRequest? Body { get; init; }
}
