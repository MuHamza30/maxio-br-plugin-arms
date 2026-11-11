namespace MaxioAdvancedBilling.Requests.SubscriptionNotes;

/// <summary>
/// The inputs of the DeleteSubscriptionNote operation.
/// </summary>
public sealed record DeleteSubscriptionNoteRequest
{
    /// <summary>
    /// The Chargify id of the subscription.
    /// </summary>
    public required int SubscriptionId { get; init; }

    /// <summary>
    /// The Advanced Billing id of the note
    /// </summary>
    public required int NoteId { get; init; }
}
