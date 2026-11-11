using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class CreateProductPricePointError : ApiError
{
    private readonly Optional<ProductPricePointErrorResponse1> _productPricePointErrorResponse1Value;

    private CreateProductPricePointError(Optional<ProductPricePointErrorResponse1> productPricePointErrorResponse1Value,
        Optional<RawError> fallback) : base(fallback)
    {
        _productPricePointErrorResponse1Value = productPricePointErrorResponse1Value;
    }

    private static CreateProductPricePointError AsProductPricePointErrorResponse1(ProductPricePointErrorResponse1 value) =>
        new(Optional<ProductPricePointErrorResponse1>.Some(value), default);

    private static CreateProductPricePointError AsFallback(RawError value) =>
        new(default, Optional<RawError>.Some(value));

    public bool TryGetProductPricePointErrorResponse1(out ProductPricePointErrorResponse1 value) =>
        _productPricePointErrorResponse1Value.TryGetValue(out value);

    private static Task<CreateProductPricePointError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            422 => response.Json<ProductPricePointErrorResponse1>().As(AsProductPricePointErrorResponse1),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<CreateProductPricePointError> Response { get; } = new(Create);
}
