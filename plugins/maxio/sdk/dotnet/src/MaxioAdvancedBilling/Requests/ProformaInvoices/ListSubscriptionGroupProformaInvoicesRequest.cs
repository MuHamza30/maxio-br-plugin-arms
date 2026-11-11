namespace MaxioAdvancedBilling.Requests.ProformaInvoices;

/// <summary>
/// The inputs of the ListSubscriptionGroupProformaInvoices operation.
/// </summary>
public sealed record ListSubscriptionGroupProformaInvoicesRequest
{
    /// <summary>
    /// The uid of the subscription group
    /// </summary>
    public required string Uid { get; init; }

    /// <summary>
    /// Include line items data.
    /// </summary>
    public bool LineItems { get; init; } = false;

    /// <summary>
    /// Include discounts data.
    /// </summary>
    public bool Discounts { get; init; } = false;

    /// <summary>
    /// Include taxes data.
    /// </summary>
    public bool Taxes { get; init; } = false;

    /// <summary>
    /// Include credits data.
    /// </summary>
    public bool Credits { get; init; } = false;

    /// <summary>
    /// Include payments data.
    /// </summary>
    public bool Payments { get; init; } = false;

    /// <summary>
    /// Include custom fields data.
    /// </summary>
    public bool CustomFields { get; init; } = false;
}
