namespace MaxioAdvancedBilling.Requests.Offers;

/// <summary>
/// The inputs of the ArchiveOffer operation.
/// </summary>
public sealed record ArchiveOfferRequest
{
    /// <summary>
    /// The Chargify id of the offer
    /// </summary>
    public required int OfferId { get; init; }
}
