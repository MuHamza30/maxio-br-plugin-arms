using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.SubscriptionNotes;

/// <summary>
/// The inputs of the UpdateSubscriptionNote operation.
/// </summary>
public sealed record UpdateSubscriptionNoteOperationRequest
{
    /// <summary>
    /// The Chargify id of the subscription.
    /// </summary>
    public required int SubscriptionId { get; init; }

    /// <summary>
    /// The Advanced Billing id of the note
    /// </summary>
    public required int NoteId { get; init; }

    public UpdateSubscriptionNoteRequest? Body { get; init; }
}
