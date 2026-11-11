using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// The vault that stores the payment profile with the provided vault_token. Use <c>bogus</c> for testing.
/// </summary>
[JsonConverter(typeof(StringEnumConverter<BankAccountVault>))]
public sealed record BankAccountVault : OpenStringEnum<BankAccountVault>
{
    private BankAccountVault(string value) : base(value)
    {
    }

    public static readonly BankAccountVault Authorizenet = new("authorizenet");

    public static readonly BankAccountVault BlueSnap = new("blue_snap");

    public static readonly BankAccountVault Bogus = new("bogus");

    public static readonly BankAccountVault Forte = new("forte");

    public static readonly BankAccountVault Gocardless = new("gocardless");

    public static readonly BankAccountVault MaxioPayments = new("maxio_payments");

    public static readonly BankAccountVault Maxp = new("maxp");

    public static readonly BankAccountVault StripeConnect = new("stripe_connect");

    public TResult Match<TResult>(Func<TResult> onAuthorizenet,
        Func<TResult> onBlueSnap,
        Func<TResult> onBogus,
        Func<TResult> onForte,
        Func<TResult> onGocardless,
        Func<TResult> onMaxioPayments,
        Func<TResult> onMaxp,
        Func<TResult> onStripeConnect,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Authorizenet => onAuthorizenet(),
            _ when this == BlueSnap => onBlueSnap(),
            _ when this == Bogus => onBogus(),
            _ when this == Forte => onForte(),
            _ when this == Gocardless => onGocardless(),
            _ when this == MaxioPayments => onMaxioPayments(),
            _ when this == Maxp => onMaxp(),
            _ when this == StripeConnect => onStripeConnect(),
            _ => otherwise(Value)
        };

    public void Match(Action onAuthorizenet,
        Action onBlueSnap,
        Action onBogus,
        Action onForte,
        Action onGocardless,
        Action onMaxioPayments,
        Action onMaxp,
        Action onStripeConnect,
        Action<string> otherwise)
    {
        if (this == Authorizenet) onAuthorizenet();
        else if (this == BlueSnap) onBlueSnap();
        else if (this == Bogus) onBogus();
        else if (this == Forte) onForte();
        else if (this == Gocardless) onGocardless();
        else if (this == MaxioPayments) onMaxioPayments();
        else if (this == Maxp) onMaxp();
        else if (this == StripeConnect) onStripeConnect();
        else otherwise(Value);
    }
}
