using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class BulkCreateProductPricePointsError : ApiError
{
    private readonly Optional<IReadOnlyDictionary<string, JsonElement>> _mapOfJsonElementValue;

    private BulkCreateProductPricePointsError(Optional<IReadOnlyDictionary<string, JsonElement>> mapOfJsonElementValue,
        Optional<RawError> fallback) : base(fallback)
    {
        _mapOfJsonElementValue = mapOfJsonElementValue;
    }

    private static BulkCreateProductPricePointsError AsMapOfJsonElement(IReadOnlyDictionary<string, JsonElement> value) =>
        new(Optional<IReadOnlyDictionary<string, JsonElement>>.Some(value), default);

    private static BulkCreateProductPricePointsError AsFallback(RawError value) =>
        new(default, Optional<RawError>.Some(value));

    public bool TryGetMapOfJsonElement(out IReadOnlyDictionary<string, JsonElement> value) =>
        _mapOfJsonElementValue.TryGetValue(out value);

    private static Task<BulkCreateProductPricePointsError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            422 => response.Json<IReadOnlyDictionary<string, JsonElement>>().As(AsMapOfJsonElement),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<BulkCreateProductPricePointsError> Response { get; } = new(Create);
}
