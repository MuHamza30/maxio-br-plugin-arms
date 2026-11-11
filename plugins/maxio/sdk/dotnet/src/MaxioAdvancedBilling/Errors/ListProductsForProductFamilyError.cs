using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class ListProductsForProductFamilyError : ApiError
{
    private readonly Optional<string> _stringValue;

    private ListProductsForProductFamilyError(Optional<string> stringValue,
        Optional<RawError> fallback) : base(fallback)
    {
        _stringValue = stringValue;
    }

    private static ListProductsForProductFamilyError AsString(string value) =>
        new(Optional<string>.Some(value), default);

    private static ListProductsForProductFamilyError AsFallback(RawError value) =>
        new(default, Optional<RawError>.Some(value));

    public bool TryGetString(out string value) => _stringValue.TryGetValue(out value);

    private static Task<ListProductsForProductFamilyError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            404 => response.Json<string>().As(AsString),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<ListProductsForProductFamilyError> Response { get; } = new(Create);
}
