using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class PreviewAllocationsError : ApiError
{
    private readonly Optional<ComponentAllocationError1> _componentAllocationError1Value;

    private PreviewAllocationsError(Optional<ComponentAllocationError1> componentAllocationError1Value,
        Optional<RawError> fallback) : base(fallback)
    {
        _componentAllocationError1Value = componentAllocationError1Value;
    }

    private static PreviewAllocationsError AsComponentAllocationError1(ComponentAllocationError1 value) =>
        new(Optional<ComponentAllocationError1>.Some(value), default);

    private static PreviewAllocationsError AsFallback(RawError value) => new(default, Optional<RawError>.Some(value));

    public bool TryGetComponentAllocationError1(out ComponentAllocationError1 value) =>
        _componentAllocationError1Value.TryGetValue(out value);

    private static Task<PreviewAllocationsError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            422 => response.Json<ComponentAllocationError1>().As(AsComponentAllocationError1),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<PreviewAllocationsError> Response { get; } = new(Create);
}
