using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

    public bool TryGetMapOfJsonElement([NotNullWhen(true)] out IReadOnlyDictionary<string, JsonElement>? value) =>
        _mapOfJsonElementValue.TryGetValue(out value);

    public TResult Match<TResult>(Func<IReadOnlyDictionary<string, JsonElement>, TResult> onMapOfJsonElement,
        Func<RawError, TResult> otherwise) =>
        this switch
        {
            _ when TryGetMapOfJsonElement(out var mapOfJsonElementValue) => onMapOfJsonElement(mapOfJsonElementValue),
            _ when TryGetRawError(out var fallback) => otherwise(fallback),
            _ => throw new InvalidOperationException($"{nameof(BulkCreateProductPricePointsError)} holds no value.")
        };

    public void Match(Action<IReadOnlyDictionary<string, JsonElement>> onMapOfJsonElement, Action<RawError> otherwise)
    {
        if (TryGetMapOfJsonElement(out var mapOfJsonElementValue)) onMapOfJsonElement(mapOfJsonElementValue);
        else if (TryGetRawError(out var fallback)) otherwise(fallback);
        else throw new InvalidOperationException($"{nameof(BulkCreateProductPricePointsError)} holds no value.");
    }

    private static Task<BulkCreateProductPricePointsError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            422 => response.Json<IReadOnlyDictionary<string, JsonElement>>().As(AsMapOfJsonElement),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<BulkCreateProductPricePointsError> Response { get; } = new(Create);
}
