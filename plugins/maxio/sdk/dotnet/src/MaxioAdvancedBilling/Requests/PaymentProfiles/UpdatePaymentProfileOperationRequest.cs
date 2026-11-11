using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.PaymentProfiles;

/// <summary>
/// The inputs of the UpdatePaymentProfile operation.
/// </summary>
public sealed record UpdatePaymentProfileOperationRequest
{
    /// <summary>
    /// The Chargify id of the payment profile
    /// </summary>
    public required int PaymentProfileId { get; init; }

    public UpdatePaymentProfileRequest? Body { get; init; }
}
