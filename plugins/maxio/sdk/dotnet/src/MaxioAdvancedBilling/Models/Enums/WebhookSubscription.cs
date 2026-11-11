using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<WebhookSubscription>))]
public sealed record WebhookSubscription : OpenStringEnum<WebhookSubscription>
{
    private WebhookSubscription(string value) : base(value)
    {
    }

    public static readonly WebhookSubscription BillingDateChange = new("billing_date_change");

    public static readonly WebhookSubscription ComponentAllocationChange = new("component_allocation_change");

    public static readonly WebhookSubscription ChjsTokenizationFailure = new("chjs_tokenization_failure");

    public static readonly WebhookSubscription ChjsTokenizationSuccess = new("chjs_tokenization_success");

    public static readonly WebhookSubscription CustomerCreate = new("customer_create");

    public static readonly WebhookSubscription CustomerUpdate = new("customer_update");

    public static readonly WebhookSubscription DunningStepReached = new("dunning_step_reached");

    public static readonly WebhookSubscription ExpiringCard = new("expiring_card");

    public static readonly WebhookSubscription ExpirationDateChange = new("expiration_date_change");

    public static readonly WebhookSubscription InvoiceIssued = new("invoice_issued");

    public static readonly WebhookSubscription InvoicePending = new("invoice_pending");

    public static readonly WebhookSubscription MeteredUsage = new("metered_usage");

    public static readonly WebhookSubscription PaymentFailure = new("payment_failure");

    public static readonly WebhookSubscription PaymentSuccess = new("payment_success");

    public static readonly WebhookSubscription DirectDebitPaymentPending = new("direct_debit_payment_pending");

    public static readonly WebhookSubscription DirectDebitPaymentPaidOut = new("direct_debit_payment_paid_out");

    public static readonly WebhookSubscription DirectDebitPaymentRejected = new("direct_debit_payment_rejected");

    public static readonly WebhookSubscription PrepaidSubscriptionBalanceChanged = new(
        "prepaid_subscription_balance_changed");

    public static readonly WebhookSubscription PrepaidUsage = new("prepaid_usage");

    public static readonly WebhookSubscription RefundFailure = new("refund_failure");

    public static readonly WebhookSubscription RefundSuccess = new("refund_success");

    public static readonly WebhookSubscription RenewalFailure = new("renewal_failure");

    public static readonly WebhookSubscription RenewalSuccess = new("renewal_success");

    public static readonly WebhookSubscription SignupFailure = new("signup_failure");

    public static readonly WebhookSubscription SignupSuccess = new("signup_success");

    public static readonly WebhookSubscription StatementClosed = new("statement_closed");

    public static readonly WebhookSubscription StatementSettled = new("statement_settled");

    public static readonly WebhookSubscription SubscriptionCardUpdate = new("subscription_card_update");

    public static readonly WebhookSubscription SubscriptionGroupCardUpdate = new("subscription_group_card_update");

    public static readonly WebhookSubscription SubscriptionProductChange = new("subscription_product_change");

    public static readonly WebhookSubscription SubscriptionProductChangeScheduled = new(
        "subscription_product_change_scheduled");

    public static readonly WebhookSubscription SubscriptionStateChange = new("subscription_state_change");

    public static readonly WebhookSubscription TrialEndNotice = new("trial_end_notice");

    public static readonly WebhookSubscription UpcomingRenewalNotice = new("upcoming_renewal_notice");

    public static readonly WebhookSubscription UpgradeDowngradeFailure = new("upgrade_downgrade_failure");

    public static readonly WebhookSubscription UpgradeDowngradeSuccess = new("upgrade_downgrade_success");

    public static readonly WebhookSubscription PendingCancellationChange = new("pending_cancellation_change");

    public static readonly WebhookSubscription SubscriptionPrepaymentAccountBalanceChanged = new(
        "subscription_prepayment_account_balance_changed");

    public static readonly WebhookSubscription SubscriptionServiceCreditAccountBalanceChanged = new(
        "subscription_service_credit_account_balance_changed");

    public TResult Match<TResult>(Func<TResult> onBillingDateChange,
        Func<TResult> onComponentAllocationChange,
        Func<TResult> onChjsTokenizationFailure,
        Func<TResult> onChjsTokenizationSuccess,
        Func<TResult> onCustomerCreate,
        Func<TResult> onCustomerUpdate,
        Func<TResult> onDunningStepReached,
        Func<TResult> onExpiringCard,
        Func<TResult> onExpirationDateChange,
        Func<TResult> onInvoiceIssued,
        Func<TResult> onInvoicePending,
        Func<TResult> onMeteredUsage,
        Func<TResult> onPaymentFailure,
        Func<TResult> onPaymentSuccess,
        Func<TResult> onDirectDebitPaymentPending,
        Func<TResult> onDirectDebitPaymentPaidOut,
        Func<TResult> onDirectDebitPaymentRejected,
        Func<TResult> onPrepaidSubscriptionBalanceChanged,
        Func<TResult> onPrepaidUsage,
        Func<TResult> onRefundFailure,
        Func<TResult> onRefundSuccess,
        Func<TResult> onRenewalFailure,
        Func<TResult> onRenewalSuccess,
        Func<TResult> onSignupFailure,
        Func<TResult> onSignupSuccess,
        Func<TResult> onStatementClosed,
        Func<TResult> onStatementSettled,
        Func<TResult> onSubscriptionCardUpdate,
        Func<TResult> onSubscriptionGroupCardUpdate,
        Func<TResult> onSubscriptionProductChange,
        Func<TResult> onSubscriptionProductChangeScheduled,
        Func<TResult> onSubscriptionStateChange,
        Func<TResult> onTrialEndNotice,
        Func<TResult> onUpcomingRenewalNotice,
        Func<TResult> onUpgradeDowngradeFailure,
        Func<TResult> onUpgradeDowngradeSuccess,
        Func<TResult> onPendingCancellationChange,
        Func<TResult> onSubscriptionPrepaymentAccountBalanceChanged,
        Func<TResult> onSubscriptionServiceCreditAccountBalanceChanged,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == BillingDateChange => onBillingDateChange(),
            _ when this == ComponentAllocationChange => onComponentAllocationChange(),
            _ when this == ChjsTokenizationFailure => onChjsTokenizationFailure(),
            _ when this == ChjsTokenizationSuccess => onChjsTokenizationSuccess(),
            _ when this == CustomerCreate => onCustomerCreate(),
            _ when this == CustomerUpdate => onCustomerUpdate(),
            _ when this == DunningStepReached => onDunningStepReached(),
            _ when this == ExpiringCard => onExpiringCard(),
            _ when this == ExpirationDateChange => onExpirationDateChange(),
            _ when this == InvoiceIssued => onInvoiceIssued(),
            _ when this == InvoicePending => onInvoicePending(),
            _ when this == MeteredUsage => onMeteredUsage(),
            _ when this == PaymentFailure => onPaymentFailure(),
            _ when this == PaymentSuccess => onPaymentSuccess(),
            _ when this == DirectDebitPaymentPending => onDirectDebitPaymentPending(),
            _ when this == DirectDebitPaymentPaidOut => onDirectDebitPaymentPaidOut(),
            _ when this == DirectDebitPaymentRejected => onDirectDebitPaymentRejected(),
            _ when this == PrepaidSubscriptionBalanceChanged => onPrepaidSubscriptionBalanceChanged(),
            _ when this == PrepaidUsage => onPrepaidUsage(),
            _ when this == RefundFailure => onRefundFailure(),
            _ when this == RefundSuccess => onRefundSuccess(),
            _ when this == RenewalFailure => onRenewalFailure(),
            _ when this == RenewalSuccess => onRenewalSuccess(),
            _ when this == SignupFailure => onSignupFailure(),
            _ when this == SignupSuccess => onSignupSuccess(),
            _ when this == StatementClosed => onStatementClosed(),
            _ when this == StatementSettled => onStatementSettled(),
            _ when this == SubscriptionCardUpdate => onSubscriptionCardUpdate(),
            _ when this == SubscriptionGroupCardUpdate => onSubscriptionGroupCardUpdate(),
            _ when this == SubscriptionProductChange => onSubscriptionProductChange(),
            _ when this == SubscriptionProductChangeScheduled => onSubscriptionProductChangeScheduled(),
            _ when this == SubscriptionStateChange => onSubscriptionStateChange(),
            _ when this == TrialEndNotice => onTrialEndNotice(),
            _ when this == UpcomingRenewalNotice => onUpcomingRenewalNotice(),
            _ when this == UpgradeDowngradeFailure => onUpgradeDowngradeFailure(),
            _ when this == UpgradeDowngradeSuccess => onUpgradeDowngradeSuccess(),
            _ when this == PendingCancellationChange => onPendingCancellationChange(),
            _ when this ==
                SubscriptionPrepaymentAccountBalanceChanged => onSubscriptionPrepaymentAccountBalanceChanged(),
            _ when this ==
                SubscriptionServiceCreditAccountBalanceChanged => onSubscriptionServiceCreditAccountBalanceChanged(),
            _ => otherwise(Value)
        };

    public void Match(Action onBillingDateChange,
        Action onComponentAllocationChange,
        Action onChjsTokenizationFailure,
        Action onChjsTokenizationSuccess,
        Action onCustomerCreate,
        Action onCustomerUpdate,
        Action onDunningStepReached,
        Action onExpiringCard,
        Action onExpirationDateChange,
        Action onInvoiceIssued,
        Action onInvoicePending,
        Action onMeteredUsage,
        Action onPaymentFailure,
        Action onPaymentSuccess,
        Action onDirectDebitPaymentPending,
        Action onDirectDebitPaymentPaidOut,
        Action onDirectDebitPaymentRejected,
        Action onPrepaidSubscriptionBalanceChanged,
        Action onPrepaidUsage,
        Action onRefundFailure,
        Action onRefundSuccess,
        Action onRenewalFailure,
        Action onRenewalSuccess,
        Action onSignupFailure,
        Action onSignupSuccess,
        Action onStatementClosed,
        Action onStatementSettled,
        Action onSubscriptionCardUpdate,
        Action onSubscriptionGroupCardUpdate,
        Action onSubscriptionProductChange,
        Action onSubscriptionProductChangeScheduled,
        Action onSubscriptionStateChange,
        Action onTrialEndNotice,
        Action onUpcomingRenewalNotice,
        Action onUpgradeDowngradeFailure,
        Action onUpgradeDowngradeSuccess,
        Action onPendingCancellationChange,
        Action onSubscriptionPrepaymentAccountBalanceChanged,
        Action onSubscriptionServiceCreditAccountBalanceChanged,
        Action<string> otherwise)
    {
        if (this == BillingDateChange) onBillingDateChange();
        else if (this == ComponentAllocationChange) onComponentAllocationChange();
        else if (this == ChjsTokenizationFailure) onChjsTokenizationFailure();
        else if (this == ChjsTokenizationSuccess) onChjsTokenizationSuccess();
        else if (this == CustomerCreate) onCustomerCreate();
        else if (this == CustomerUpdate) onCustomerUpdate();
        else if (this == DunningStepReached) onDunningStepReached();
        else if (this == ExpiringCard) onExpiringCard();
        else if (this == ExpirationDateChange) onExpirationDateChange();
        else if (this == InvoiceIssued) onInvoiceIssued();
        else if (this == InvoicePending) onInvoicePending();
        else if (this == MeteredUsage) onMeteredUsage();
        else if (this == PaymentFailure) onPaymentFailure();
        else if (this == PaymentSuccess) onPaymentSuccess();
        else if (this == DirectDebitPaymentPending) onDirectDebitPaymentPending();
        else if (this == DirectDebitPaymentPaidOut) onDirectDebitPaymentPaidOut();
        else if (this == DirectDebitPaymentRejected) onDirectDebitPaymentRejected();
        else if (this == PrepaidSubscriptionBalanceChanged) onPrepaidSubscriptionBalanceChanged();
        else if (this == PrepaidUsage) onPrepaidUsage();
        else if (this == RefundFailure) onRefundFailure();
        else if (this == RefundSuccess) onRefundSuccess();
        else if (this == RenewalFailure) onRenewalFailure();
        else if (this == RenewalSuccess) onRenewalSuccess();
        else if (this == SignupFailure) onSignupFailure();
        else if (this == SignupSuccess) onSignupSuccess();
        else if (this == StatementClosed) onStatementClosed();
        else if (this == StatementSettled) onStatementSettled();
        else if (this == SubscriptionCardUpdate) onSubscriptionCardUpdate();
        else if (this == SubscriptionGroupCardUpdate) onSubscriptionGroupCardUpdate();
        else if (this == SubscriptionProductChange) onSubscriptionProductChange();
        else if (this == SubscriptionProductChangeScheduled) onSubscriptionProductChangeScheduled();
        else if (this == SubscriptionStateChange) onSubscriptionStateChange();
        else if (this == TrialEndNotice) onTrialEndNotice();
        else if (this == UpcomingRenewalNotice) onUpcomingRenewalNotice();
        else if (this == UpgradeDowngradeFailure) onUpgradeDowngradeFailure();
        else if (this == UpgradeDowngradeSuccess) onUpgradeDowngradeSuccess();
        else if (this == PendingCancellationChange) onPendingCancellationChange();
        else if (this == SubscriptionPrepaymentAccountBalanceChanged) onSubscriptionPrepaymentAccountBalanceChanged();
        else if (this ==
            SubscriptionServiceCreditAccountBalanceChanged) onSubscriptionServiceCreditAccountBalanceChanged();
        else otherwise(Value);
    }
}
