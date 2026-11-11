using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class UpdateMetadataError : ApiError
{
    private readonly Optional<SingleErrorResponse1> _singleErrorResponse1Value;

    private UpdateMetadataError(Optional<SingleErrorResponse1> singleErrorResponse1Value,
        Optional<RawError> fallback) : base(fallback)
    {
        _singleErrorResponse1Value = singleErrorResponse1Value;
    }

    private static UpdateMetadataError AsSingleErrorResponse1(SingleErrorResponse1 value) =>
        new(Optional<SingleErrorResponse1>.Some(value), default);

    private static UpdateMetadataError AsFallback(RawError value) => new(default, Optional<RawError>.Some(value));

    public bool TryGetSingleErrorResponse1(out SingleErrorResponse1 value) =>
        _singleErrorResponse1Value.TryGetValue(out value);

    private static Task<UpdateMetadataError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            422 => response.Json<SingleErrorResponse1>().As(AsSingleErrorResponse1),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<UpdateMetadataError> Response { get; } = new(Create);
}
