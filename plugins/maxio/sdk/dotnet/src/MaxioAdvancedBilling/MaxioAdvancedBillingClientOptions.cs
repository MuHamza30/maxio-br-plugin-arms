using System;
using System.Collections.Generic;
using MaxioAdvancedBilling.Core.Authentication;
using MaxioAdvancedBilling.Core.Authentication.Basic;
using MaxioAdvancedBilling.Core.Configuration;
using MaxioAdvancedBilling.Core.Hooks;
using MaxioAdvancedBilling.Servers;

namespace MaxioAdvancedBilling;

public class MaxioAdvancedBillingClientOptions
{
    public ServerEnvironment Environment { get; set; } = ServerEnvironment.Default();
    public RetryOptions Retry { get; set; } = RetryOptions.Default();
    public LoggingOptions Logging { get; set; } = new();
    public TimeProvider TimeProvider { get; set; } = TimeProvider.System;
    public ServerOptions Server { get; set; } = new();
    /// <summary>
    /// Maximum time to wait for the next frame of a streaming (SSE) response before the stream is
    /// torn down with a timeout. Bounds only the wait for the server between frames, never the
    /// caller's own processing time. Set to null to wait indefinitely.
    /// </summary>
    public TimeSpan? StreamReadTimeout { get; set; } = TimeSpan.FromSeconds(60);
    public IReadOnlyList<SdkHook> Hooks { get; set; } = [];
    /// <summary>
    /// The <c>username</c> is a Maxio Chargify API key. The <c>password</c> is <c>x</c>.
    /// </summary>
    public BasicAuthCredentials? BasicAuth { get; set; }
    public CredentialProvider<BasicAuthCredentials>? BasicAuthProvider { get; set; }
}
