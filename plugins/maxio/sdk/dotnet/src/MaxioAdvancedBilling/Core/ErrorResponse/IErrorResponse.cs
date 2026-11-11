using System.Threading;
using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Core.ErrorResponse;

internal interface IErrorResponse<TError>
{
    Task<TError> Map(ResponseContext context, CancellationToken cancellationToken);
}
