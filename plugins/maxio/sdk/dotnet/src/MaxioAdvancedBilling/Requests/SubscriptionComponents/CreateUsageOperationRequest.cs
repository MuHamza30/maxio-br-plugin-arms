using MaxioAdvancedBilling.Models;
using MaxioAdvancedBilling.Models.AnyOf;

namespace MaxioAdvancedBilling.Requests.SubscriptionComponents;

/// <summary>
/// The inputs of the CreateUsage operation.
/// </summary>
public sealed record CreateUsageOperationRequest
{
    /// <summary>
    /// Either the Advanced Billing subscription ID (integer) or the subscription reference (string). Important: In cases where a numeric string value matches both an existing subscription ID and an existing subscription reference, the system will prioritize the subscription ID lookup. For example, if both subscription ID 123 and subscription reference "123" exist, passing "123" will return the subscription with ID 123.
    /// </summary>
    public required SubscriptionIdOrReference SubscriptionIdOrReference { get; init; }

    /// <summary>
    /// Either the Advanced Billing id for the component or the component's handle prefixed by <c>handle:</c>
    /// </summary>
    public required ComponentIdModel ComponentId { get; init; }

    public CreateUsageRequest? Body { get; init; }
}
