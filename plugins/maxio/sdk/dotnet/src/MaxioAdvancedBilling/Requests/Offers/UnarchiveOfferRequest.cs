namespace MaxioAdvancedBilling.Requests.Offers;

/// <summary>
/// The inputs of the UnarchiveOffer operation.
/// </summary>
public sealed record UnarchiveOfferRequest
{
    /// <summary>
    /// The Chargify id of the offer
    /// </summary>
    public required int OfferId { get; init; }
}
