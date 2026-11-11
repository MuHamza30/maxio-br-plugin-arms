using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using MaxioAdvancedBilling.Core.Hooks;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Core;

public sealed record RequestOptions
{
    public LogLevel? LogLevel { get; init; }

    public IReadOnlyList<HeaderParam>? Headers { get; init; }

    public IReadOnlyList<SdkHook>? Hooks { get; init; }
}
