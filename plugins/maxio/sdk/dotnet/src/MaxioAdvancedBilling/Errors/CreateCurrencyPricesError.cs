using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class CreateCurrencyPricesError : ApiError
{
    private readonly Optional<ErrorArrayMapResponse1> _errorArrayMapResponse1Value;

    private CreateCurrencyPricesError(Optional<ErrorArrayMapResponse1> errorArrayMapResponse1Value,
        Optional<RawError> fallback) : base(fallback)
    {
        _errorArrayMapResponse1Value = errorArrayMapResponse1Value;
    }

    private static CreateCurrencyPricesError AsErrorArrayMapResponse1(ErrorArrayMapResponse1 value) =>
        new(Optional<ErrorArrayMapResponse1>.Some(value), default);

    private static CreateCurrencyPricesError AsFallback(RawError value) => new(default, Optional<RawError>.Some(value));

    public bool TryGetErrorArrayMapResponse1(out ErrorArrayMapResponse1 value) =>
        _errorArrayMapResponse1Value.TryGetValue(out value);

    private static Task<CreateCurrencyPricesError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            422 => response.Json<ErrorArrayMapResponse1>().As(AsErrorArrayMapResponse1),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<CreateCurrencyPricesError> Response { get; } = new(Create);
}
