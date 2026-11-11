using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.ProductFamilies;

/// <summary>
/// The inputs of the CreateProductFamily operation.
/// </summary>
public sealed record CreateProductFamilyOperationRequest
{
    public CreateProductFamilyRequest? Body { get; init; }
}
