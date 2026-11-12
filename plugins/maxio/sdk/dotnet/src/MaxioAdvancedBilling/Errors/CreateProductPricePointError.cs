using System;
using System.Diagnostics.CodeAnalysis;
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

    public bool TryGetProductPricePointErrorResponse1([NotNullWhen(true)] out ProductPricePointErrorResponse1? value) =>
        _productPricePointErrorResponse1Value.TryGetValue(out value);

    public TResult Match<TResult>(Func<ProductPricePointErrorResponse1, TResult> onProductPricePointErrorResponse1,
        Func<RawError, TResult> otherwise) =>
        this switch
        {
            _ when TryGetProductPricePointErrorResponse1(
                out var productPricePointErrorResponse1Value) => onProductPricePointErrorResponse1(
                productPricePointErrorResponse1Value),
            _ when TryGetRawError(out var fallback) => otherwise(fallback),
            _ => throw new InvalidOperationException($"{nameof(CreateProductPricePointError)} holds no value.")
        };

    public void Match(Action<ProductPricePointErrorResponse1> onProductPricePointErrorResponse1,
        Action<RawError> otherwise)
    {
        if (TryGetProductPricePointErrorResponse1(
            out var productPricePointErrorResponse1Value)) onProductPricePointErrorResponse1(
            productPricePointErrorResponse1Value);
        else if (TryGetRawError(out var fallback)) otherwise(fallback);
        else throw new InvalidOperationException($"{nameof(CreateProductPricePointError)} holds no value.");
    }

    private static Task<CreateProductPricePointError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            422 => response.Json<ProductPricePointErrorResponse1>().As(AsProductPricePointErrorResponse1),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<CreateProductPricePointError> Response { get; } = new(Create);
}
