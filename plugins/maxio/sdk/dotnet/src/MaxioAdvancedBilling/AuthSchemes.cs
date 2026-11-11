using MaxioAdvancedBilling.Core.Authentication;
using MaxioAdvancedBilling.Core.Authentication.Basic;

namespace MaxioAdvancedBilling;

internal sealed class AuthSchemes
{
    public IAuthScheme BasicAuth { get; }

    public AuthSchemes(MaxioAdvancedBillingClientOptions options)
    {
        BasicAuth =
            BasicAuthScheme.Create(
                "BasicAuth",
                options.BasicAuthProvider,
                options.BasicAuth.WithEnvironmentDefaults(
                    "MAXIO_ADVANCED_BILLING_BASIC_AUTH_USERNAME",
                    "MAXIO_ADVANCED_BILLING_BASIC_AUTH_PASSWORD"));
    }
}
