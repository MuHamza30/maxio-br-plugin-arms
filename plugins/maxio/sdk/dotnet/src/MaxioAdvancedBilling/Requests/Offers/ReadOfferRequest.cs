namespace MaxioAdvancedBilling.Requests.Offers;

/// <summary>
/// The inputs of the ReadOffer operation.
/// </summary>
public sealed record ReadOfferRequest
{
    /// <summary>
    /// The Chargify id of the offer
    /// </summary>
    public required int OfferId { get; init; }
}
