using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.PaymentProfiles;

/// <summary>
/// The inputs of the CreatePaymentProfile operation.
/// </summary>
public sealed record CreatePaymentProfileOperationRequest
{
    /// <summary>
    /// When following the IBAN or the Local Bank details examples, a customer, bank account and mandate will be created in your current vault. If the customer, bank account, and mandate already exist in your vault, follow the Import example to link the payment profile into Advanced Billing.
    /// </summary>
    public CreatePaymentProfileRequest? Body { get; init; }
}
