using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// The vault that stores the payment profile with the provided <c>vault_token</c>. Use <c>bogus</c> for testing.
/// </summary>
[JsonConverter(typeof(StringEnumConverter<CreditCardVault>))]
public sealed record CreditCardVault : OpenStringEnum<CreditCardVault>
{
    private CreditCardVault(string value) : base(value)
    {
    }

    public static readonly CreditCardVault Adyen = new("adyen");

    public static readonly CreditCardVault Authorizenet = new("authorizenet");

    public static readonly CreditCardVault Beanstream = new("beanstream");

    public static readonly CreditCardVault BlueSnap = new("blue_snap");

    public static readonly CreditCardVault Bogus = new("bogus");

    public static readonly CreditCardVault Braintree1 = new("braintree1");

    public static readonly CreditCardVault BraintreeBlue = new("braintree_blue");

    public static readonly CreditCardVault Checkout = new("checkout");

    public static readonly CreditCardVault Cybersource = new("cybersource");

    public static readonly CreditCardVault Elavon = new("elavon");

    public static readonly CreditCardVault Eway = new("eway");

    public static readonly CreditCardVault EwayRapid = new("eway_rapid");

    public static readonly CreditCardVault EwayRapidStd = new("eway_rapid_std");

    public static readonly CreditCardVault Firstdata = new("firstdata");

    public static readonly CreditCardVault Forte = new("forte");

    public static readonly CreditCardVault Litle = new("litle");

    public static readonly CreditCardVault MaxioPayments = new("maxio_payments");

    public static readonly CreditCardVault Maxp = new("maxp");

    public static readonly CreditCardVault Moduslink = new("moduslink");

    public static readonly CreditCardVault Moneris = new("moneris");

    public static readonly CreditCardVault Nmi = new("nmi");

    public static readonly CreditCardVault Orbital = new("orbital");

    public static readonly CreditCardVault PaymentExpress = new("payment_express");

    public static readonly CreditCardVault Paymill = new("paymill");

    public static readonly CreditCardVault Paypal = new("paypal");

    public static readonly CreditCardVault PaypalComplete = new("paypal_complete");

    public static readonly CreditCardVault Pin = new("pin");

    public static readonly CreditCardVault Square = new("square");

    public static readonly CreditCardVault Stripe = new("stripe");

    public static readonly CreditCardVault StripeConnect = new("stripe_connect");

    public static readonly CreditCardVault TrustCommerce = new("trust_commerce");

    public static readonly CreditCardVault Unipaas = new("unipaas");

    public static readonly CreditCardVault Wirecard = new("wirecard");

    public TResult Match<TResult>(Func<TResult> onAdyen,
        Func<TResult> onAuthorizenet,
        Func<TResult> onBeanstream,
        Func<TResult> onBlueSnap,
        Func<TResult> onBogus,
        Func<TResult> onBraintree1,
        Func<TResult> onBraintreeBlue,
        Func<TResult> onCheckout,
        Func<TResult> onCybersource,
        Func<TResult> onElavon,
        Func<TResult> onEway,
        Func<TResult> onEwayRapid,
        Func<TResult> onEwayRapidStd,
        Func<TResult> onFirstdata,
        Func<TResult> onForte,
        Func<TResult> onLitle,
        Func<TResult> onMaxioPayments,
        Func<TResult> onMaxp,
        Func<TResult> onModuslink,
        Func<TResult> onMoneris,
        Func<TResult> onNmi,
        Func<TResult> onOrbital,
        Func<TResult> onPaymentExpress,
        Func<TResult> onPaymill,
        Func<TResult> onPaypal,
        Func<TResult> onPaypalComplete,
        Func<TResult> onPin,
        Func<TResult> onSquare,
        Func<TResult> onStripe,
        Func<TResult> onStripeConnect,
        Func<TResult> onTrustCommerce,
        Func<TResult> onUnipaas,
        Func<TResult> onWirecard,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Adyen => onAdyen(),
            _ when this == Authorizenet => onAuthorizenet(),
            _ when this == Beanstream => onBeanstream(),
            _ when this == BlueSnap => onBlueSnap(),
            _ when this == Bogus => onBogus(),
            _ when this == Braintree1 => onBraintree1(),
            _ when this == BraintreeBlue => onBraintreeBlue(),
            _ when this == Checkout => onCheckout(),
            _ when this == Cybersource => onCybersource(),
            _ when this == Elavon => onElavon(),
            _ when this == Eway => onEway(),
            _ when this == EwayRapid => onEwayRapid(),
            _ when this == EwayRapidStd => onEwayRapidStd(),
            _ when this == Firstdata => onFirstdata(),
            _ when this == Forte => onForte(),
            _ when this == Litle => onLitle(),
            _ when this == MaxioPayments => onMaxioPayments(),
            _ when this == Maxp => onMaxp(),
            _ when this == Moduslink => onModuslink(),
            _ when this == Moneris => onMoneris(),
            _ when this == Nmi => onNmi(),
            _ when this == Orbital => onOrbital(),
            _ when this == PaymentExpress => onPaymentExpress(),
            _ when this == Paymill => onPaymill(),
            _ when this == Paypal => onPaypal(),
            _ when this == PaypalComplete => onPaypalComplete(),
            _ when this == Pin => onPin(),
            _ when this == Square => onSquare(),
            _ when this == Stripe => onStripe(),
            _ when this == StripeConnect => onStripeConnect(),
            _ when this == TrustCommerce => onTrustCommerce(),
            _ when this == Unipaas => onUnipaas(),
            _ when this == Wirecard => onWirecard(),
            _ => otherwise(Value)
        };

    public void Match(Action onAdyen,
        Action onAuthorizenet,
        Action onBeanstream,
        Action onBlueSnap,
        Action onBogus,
        Action onBraintree1,
        Action onBraintreeBlue,
        Action onCheckout,
        Action onCybersource,
        Action onElavon,
        Action onEway,
        Action onEwayRapid,
        Action onEwayRapidStd,
        Action onFirstdata,
        Action onForte,
        Action onLitle,
        Action onMaxioPayments,
        Action onMaxp,
        Action onModuslink,
        Action onMoneris,
        Action onNmi,
        Action onOrbital,
        Action onPaymentExpress,
        Action onPaymill,
        Action onPaypal,
        Action onPaypalComplete,
        Action onPin,
        Action onSquare,
        Action onStripe,
        Action onStripeConnect,
        Action onTrustCommerce,
        Action onUnipaas,
        Action onWirecard,
        Action<string> otherwise)
    {
        if (this == Adyen) onAdyen();
        else if (this == Authorizenet) onAuthorizenet();
        else if (this == Beanstream) onBeanstream();
        else if (this == BlueSnap) onBlueSnap();
        else if (this == Bogus) onBogus();
        else if (this == Braintree1) onBraintree1();
        else if (this == BraintreeBlue) onBraintreeBlue();
        else if (this == Checkout) onCheckout();
        else if (this == Cybersource) onCybersource();
        else if (this == Elavon) onElavon();
        else if (this == Eway) onEway();
        else if (this == EwayRapid) onEwayRapid();
        else if (this == EwayRapidStd) onEwayRapidStd();
        else if (this == Firstdata) onFirstdata();
        else if (this == Forte) onForte();
        else if (this == Litle) onLitle();
        else if (this == MaxioPayments) onMaxioPayments();
        else if (this == Maxp) onMaxp();
        else if (this == Moduslink) onModuslink();
        else if (this == Moneris) onMoneris();
        else if (this == Nmi) onNmi();
        else if (this == Orbital) onOrbital();
        else if (this == PaymentExpress) onPaymentExpress();
        else if (this == Paymill) onPaymill();
        else if (this == Paypal) onPaypal();
        else if (this == PaypalComplete) onPaypalComplete();
        else if (this == Pin) onPin();
        else if (this == Square) onSquare();
        else if (this == Stripe) onStripe();
        else if (this == StripeConnect) onStripeConnect();
        else if (this == TrustCommerce) onTrustCommerce();
        else if (this == Unipaas) onUnipaas();
        else if (this == Wirecard) onWirecard();
        else otherwise(Value);
    }
}
