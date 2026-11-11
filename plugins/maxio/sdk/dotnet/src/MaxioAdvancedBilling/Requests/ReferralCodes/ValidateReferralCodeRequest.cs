namespace MaxioAdvancedBilling.Requests.ReferralCodes;

/// <summary>
/// The inputs of the ValidateReferralCode operation.
/// </summary>
public sealed record ValidateReferralCodeRequest
{
    /// <summary>
    /// The referral code you are trying to validate
    /// </summary>
    public required string Code { get; init; }
}
