using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<EventKey>))]
public sealed record EventKey : OpenStringEnum<EventKey>
{
    private EventKey(string value) : base(value)
    {
    }

    public static readonly EventKey PaymentSuccess = new("payment_success");

    public static readonly EventKey PaymentFailure = new("payment_failure");

    public static readonly EventKey SignupSuccess = new("signup_success");

    public static readonly EventKey SignupFailure = new("signup_failure");

    public static readonly EventKey DelayedSignupCreationSuccess = new("delayed_signup_creation_success");

    public static readonly EventKey DelayedSignupCreationFailure = new("delayed_signup_creation_failure");

    public static readonly EventKey BillingDateChange = new("billing_date_change");

    public static readonly EventKey ExpirationDateChange = new("expiration_date_change");

    public static readonly EventKey RenewalSuccess = new("renewal_success");

    public static readonly EventKey RenewalFailure = new("renewal_failure");

    public static readonly EventKey SubscriptionStateChange = new("subscription_state_change");

    public static readonly EventKey SubscriptionProductChange = new("subscription_product_change");

    public static readonly EventKey SubscriptionProductChangeScheduled = new("subscription_product_change_scheduled");

    public static readonly EventKey PendingCancellationChange = new("pending_cancellation_change");

    public static readonly EventKey ExpiringCard = new("expiring_card");

    public static readonly EventKey CustomerUpdate = new("customer_update");

    public static readonly EventKey CustomerCreate = new("customer_create");

    public static readonly EventKey CustomerDelete = new("customer_delete");

    public static readonly EventKey ComponentAllocationChange = new("component_allocation_change");

    public static readonly EventKey MeteredUsage = new("metered_usage");

    public static readonly EventKey PrepaidUsage = new("prepaid_usage");

    public static readonly EventKey UpgradeDowngradeSuccess = new("upgrade_downgrade_success");

    public static readonly EventKey UpgradeDowngradeFailure = new("upgrade_downgrade_failure");

    public static readonly EventKey StatementClosed = new("statement_closed");

    public static readonly EventKey StatementSettled = new("statement_settled");

    public static readonly EventKey SubscriptionCardUpdate = new("subscription_card_update");

    public static readonly EventKey SubscriptionGroupCardUpdate = new("subscription_group_card_update");

    public static readonly EventKey SubscriptionBankAccountUpdate = new("subscription_bank_account_update");

    public static readonly EventKey RefundSuccess = new("refund_success");

    public static readonly EventKey RefundFailure = new("refund_failure");

    public static readonly EventKey UpcomingRenewalNotice = new("upcoming_renewal_notice");

    public static readonly EventKey TrialEndNotice = new("trial_end_notice");

    public static readonly EventKey DunningStepReached = new("dunning_step_reached");

    public static readonly EventKey InvoiceIssued = new("invoice_issued");

    public static readonly EventKey InvoicePending = new("invoice_pending");

    public static readonly EventKey PrepaidSubscriptionBalanceChanged = new("prepaid_subscription_balance_changed");

    public static readonly EventKey SubscriptionGroupSignupSuccess = new("subscription_group_signup_success");

    public static readonly EventKey SubscriptionGroupSignupFailure = new("subscription_group_signup_failure");

    public static readonly EventKey DirectDebitPaymentPaidOut = new("direct_debit_payment_paid_out");

    public static readonly EventKey DirectDebitPaymentRejected = new("direct_debit_payment_rejected");

    public static readonly EventKey DirectDebitPaymentPending = new("direct_debit_payment_pending");

    public static readonly EventKey PendingPaymentCreated = new("pending_payment_created");

    public static readonly EventKey PendingPaymentFailed = new("pending_payment_failed");

    public static readonly EventKey PendingPaymentCompleted = new("pending_payment_completed");

    public static readonly EventKey ProformaInvoiceIssued = new("proforma_invoice_issued");

    public static readonly EventKey SubscriptionPrepaymentAccountBalanceChanged = new(
        "subscription_prepayment_account_balance_changed");

    public static readonly EventKey SubscriptionServiceCreditAccountBalanceChanged = new(
        "subscription_service_credit_account_balance_changed");

    public static readonly EventKey CustomFieldValueChange = new("custom_field_value_change");

    public static readonly EventKey ItemPricePointChanged = new("item_price_point_changed");

    public static readonly EventKey RenewalSuccessRecreated = new("renewal_success_recreated");

    public static readonly EventKey RenewalFailureRecreated = new("renewal_failure_recreated");

    public static readonly EventKey PaymentSuccessRecreated = new("payment_success_recreated");

    public static readonly EventKey PaymentFailureRecreated = new("payment_failure_recreated");

    public static readonly EventKey SubscriptionDeletion = new("subscription_deletion");

    public static readonly EventKey SubscriptionGroupBankAccountUpdate = new("subscription_group_bank_account_update");

    public static readonly EventKey SubscriptionPaypalAccountUpdate = new("subscription_paypal_account_update");

    public static readonly EventKey SubscriptionGroupPaypalAccountUpdate = new(
        "subscription_group_paypal_account_update");

    public static readonly EventKey SubscriptionCustomerChange = new("subscription_customer_change");

    public static readonly EventKey AccountTransactionChanged = new("account_transaction_changed");

    public static readonly EventKey GoCardlessPaymentPaidOut = new("go_cardless_payment_paid_out");

    public static readonly EventKey GoCardlessPaymentRejected = new("go_cardless_payment_rejected");

    public static readonly EventKey GoCardlessPaymentPending = new("go_cardless_payment_pending");

    public static readonly EventKey StripeDirectDebitPaymentPaidOut = new("stripe_direct_debit_payment_paid_out");

    public static readonly EventKey StripeDirectDebitPaymentRejected = new("stripe_direct_debit_payment_rejected");

    public static readonly EventKey StripeDirectDebitPaymentPending = new("stripe_direct_debit_payment_pending");

    public static readonly EventKey MaxioPaymentsDirectDebitPaymentPaidOut = new(
        "maxio_payments_direct_debit_payment_paid_out");

    public static readonly EventKey MaxioPaymentsDirectDebitPaymentRejected = new(
        "maxio_payments_direct_debit_payment_rejected");

    public static readonly EventKey MaxioPaymentsDirectDebitPaymentPending = new(
        "maxio_payments_direct_debit_payment_pending");

    public static readonly EventKey InvoiceInCollectionsCanceled = new("invoice_in_collections_canceled");

    public static readonly EventKey SubscriptionAddedToGroup = new("subscription_added_to_group");

    public static readonly EventKey SubscriptionRemovedFromGroup = new("subscription_removed_from_group");

    public static readonly EventKey ChargebackOpened = new("chargeback_opened");

    public static readonly EventKey ChargebackLost = new("chargeback_lost");

    public static readonly EventKey ChargebackAccepted = new("chargeback_accepted");

    public static readonly EventKey ChargebackClosed = new("chargeback_closed");

    public static readonly EventKey ChargebackWon = new("chargeback_won");

    public static readonly EventKey PaymentCollectionMethodChanged = new("payment_collection_method_changed");

    public static readonly EventKey ComponentBillingDateChanged = new("component_billing_date_changed");

    public static readonly EventKey ChjsTokenizationFailure = new("chjs_tokenization_failure");

    public static readonly EventKey ChjsTokenizationSuccess = new("chjs_tokenization_success");

    public static readonly EventKey SubscriptionTermRenewalScheduled = new("subscription_term_renewal_scheduled");

    public static readonly EventKey SubscriptionTermRenewalPending = new("subscription_term_renewal_pending");

    public static readonly EventKey SubscriptionTermRenewalActivated = new("subscription_term_renewal_activated");

    public static readonly EventKey SubscriptionTermRenewalRemoved = new("subscription_term_renewal_removed");

    public TResult Match<TResult>(Func<TResult> onPaymentSuccess,
        Func<TResult> onPaymentFailure,
        Func<TResult> onSignupSuccess,
        Func<TResult> onSignupFailure,
        Func<TResult> onDelayedSignupCreationSuccess,
        Func<TResult> onDelayedSignupCreationFailure,
        Func<TResult> onBillingDateChange,
        Func<TResult> onExpirationDateChange,
        Func<TResult> onRenewalSuccess,
        Func<TResult> onRenewalFailure,
        Func<TResult> onSubscriptionStateChange,
        Func<TResult> onSubscriptionProductChange,
        Func<TResult> onSubscriptionProductChangeScheduled,
        Func<TResult> onPendingCancellationChange,
        Func<TResult> onExpiringCard,
        Func<TResult> onCustomerUpdate,
        Func<TResult> onCustomerCreate,
        Func<TResult> onCustomerDelete,
        Func<TResult> onComponentAllocationChange,
        Func<TResult> onMeteredUsage,
        Func<TResult> onPrepaidUsage,
        Func<TResult> onUpgradeDowngradeSuccess,
        Func<TResult> onUpgradeDowngradeFailure,
        Func<TResult> onStatementClosed,
        Func<TResult> onStatementSettled,
        Func<TResult> onSubscriptionCardUpdate,
        Func<TResult> onSubscriptionGroupCardUpdate,
        Func<TResult> onSubscriptionBankAccountUpdate,
        Func<TResult> onRefundSuccess,
        Func<TResult> onRefundFailure,
        Func<TResult> onUpcomingRenewalNotice,
        Func<TResult> onTrialEndNotice,
        Func<TResult> onDunningStepReached,
        Func<TResult> onInvoiceIssued,
        Func<TResult> onInvoicePending,
        Func<TResult> onPrepaidSubscriptionBalanceChanged,
        Func<TResult> onSubscriptionGroupSignupSuccess,
        Func<TResult> onSubscriptionGroupSignupFailure,
        Func<TResult> onDirectDebitPaymentPaidOut,
        Func<TResult> onDirectDebitPaymentRejected,
        Func<TResult> onDirectDebitPaymentPending,
        Func<TResult> onPendingPaymentCreated,
        Func<TResult> onPendingPaymentFailed,
        Func<TResult> onPendingPaymentCompleted,
        Func<TResult> onProformaInvoiceIssued,
        Func<TResult> onSubscriptionPrepaymentAccountBalanceChanged,
        Func<TResult> onSubscriptionServiceCreditAccountBalanceChanged,
        Func<TResult> onCustomFieldValueChange,
        Func<TResult> onItemPricePointChanged,
        Func<TResult> onRenewalSuccessRecreated,
        Func<TResult> onRenewalFailureRecreated,
        Func<TResult> onPaymentSuccessRecreated,
        Func<TResult> onPaymentFailureRecreated,
        Func<TResult> onSubscriptionDeletion,
        Func<TResult> onSubscriptionGroupBankAccountUpdate,
        Func<TResult> onSubscriptionPaypalAccountUpdate,
        Func<TResult> onSubscriptionGroupPaypalAccountUpdate,
        Func<TResult> onSubscriptionCustomerChange,
        Func<TResult> onAccountTransactionChanged,
        Func<TResult> onGoCardlessPaymentPaidOut,
        Func<TResult> onGoCardlessPaymentRejected,
        Func<TResult> onGoCardlessPaymentPending,
        Func<TResult> onStripeDirectDebitPaymentPaidOut,
        Func<TResult> onStripeDirectDebitPaymentRejected,
        Func<TResult> onStripeDirectDebitPaymentPending,
        Func<TResult> onMaxioPaymentsDirectDebitPaymentPaidOut,
        Func<TResult> onMaxioPaymentsDirectDebitPaymentRejected,
        Func<TResult> onMaxioPaymentsDirectDebitPaymentPending,
        Func<TResult> onInvoiceInCollectionsCanceled,
        Func<TResult> onSubscriptionAddedToGroup,
        Func<TResult> onSubscriptionRemovedFromGroup,
        Func<TResult> onChargebackOpened,
        Func<TResult> onChargebackLost,
        Func<TResult> onChargebackAccepted,
        Func<TResult> onChargebackClosed,
        Func<TResult> onChargebackWon,
        Func<TResult> onPaymentCollectionMethodChanged,
        Func<TResult> onComponentBillingDateChanged,
        Func<TResult> onChjsTokenizationFailure,
        Func<TResult> onChjsTokenizationSuccess,
        Func<TResult> onSubscriptionTermRenewalScheduled,
        Func<TResult> onSubscriptionTermRenewalPending,
        Func<TResult> onSubscriptionTermRenewalActivated,
        Func<TResult> onSubscriptionTermRenewalRemoved,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == PaymentSuccess => onPaymentSuccess(),
            _ when this == PaymentFailure => onPaymentFailure(),
            _ when this == SignupSuccess => onSignupSuccess(),
            _ when this == SignupFailure => onSignupFailure(),
            _ when this == DelayedSignupCreationSuccess => onDelayedSignupCreationSuccess(),
            _ when this == DelayedSignupCreationFailure => onDelayedSignupCreationFailure(),
            _ when this == BillingDateChange => onBillingDateChange(),
            _ when this == ExpirationDateChange => onExpirationDateChange(),
            _ when this == RenewalSuccess => onRenewalSuccess(),
            _ when this == RenewalFailure => onRenewalFailure(),
            _ when this == SubscriptionStateChange => onSubscriptionStateChange(),
            _ when this == SubscriptionProductChange => onSubscriptionProductChange(),
            _ when this == SubscriptionProductChangeScheduled => onSubscriptionProductChangeScheduled(),
            _ when this == PendingCancellationChange => onPendingCancellationChange(),
            _ when this == ExpiringCard => onExpiringCard(),
            _ when this == CustomerUpdate => onCustomerUpdate(),
            _ when this == CustomerCreate => onCustomerCreate(),
            _ when this == CustomerDelete => onCustomerDelete(),
            _ when this == ComponentAllocationChange => onComponentAllocationChange(),
            _ when this == MeteredUsage => onMeteredUsage(),
            _ when this == PrepaidUsage => onPrepaidUsage(),
            _ when this == UpgradeDowngradeSuccess => onUpgradeDowngradeSuccess(),
            _ when this == UpgradeDowngradeFailure => onUpgradeDowngradeFailure(),
            _ when this == StatementClosed => onStatementClosed(),
            _ when this == StatementSettled => onStatementSettled(),
            _ when this == SubscriptionCardUpdate => onSubscriptionCardUpdate(),
            _ when this == SubscriptionGroupCardUpdate => onSubscriptionGroupCardUpdate(),
            _ when this == SubscriptionBankAccountUpdate => onSubscriptionBankAccountUpdate(),
            _ when this == RefundSuccess => onRefundSuccess(),
            _ when this == RefundFailure => onRefundFailure(),
            _ when this == UpcomingRenewalNotice => onUpcomingRenewalNotice(),
            _ when this == TrialEndNotice => onTrialEndNotice(),
            _ when this == DunningStepReached => onDunningStepReached(),
            _ when this == InvoiceIssued => onInvoiceIssued(),
            _ when this == InvoicePending => onInvoicePending(),
            _ when this == PrepaidSubscriptionBalanceChanged => onPrepaidSubscriptionBalanceChanged(),
            _ when this == SubscriptionGroupSignupSuccess => onSubscriptionGroupSignupSuccess(),
            _ when this == SubscriptionGroupSignupFailure => onSubscriptionGroupSignupFailure(),
            _ when this == DirectDebitPaymentPaidOut => onDirectDebitPaymentPaidOut(),
            _ when this == DirectDebitPaymentRejected => onDirectDebitPaymentRejected(),
            _ when this == DirectDebitPaymentPending => onDirectDebitPaymentPending(),
            _ when this == PendingPaymentCreated => onPendingPaymentCreated(),
            _ when this == PendingPaymentFailed => onPendingPaymentFailed(),
            _ when this == PendingPaymentCompleted => onPendingPaymentCompleted(),
            _ when this == ProformaInvoiceIssued => onProformaInvoiceIssued(),
            _ when this ==
                SubscriptionPrepaymentAccountBalanceChanged => onSubscriptionPrepaymentAccountBalanceChanged(),
            _ when this ==
                SubscriptionServiceCreditAccountBalanceChanged => onSubscriptionServiceCreditAccountBalanceChanged(),
            _ when this == CustomFieldValueChange => onCustomFieldValueChange(),
            _ when this == ItemPricePointChanged => onItemPricePointChanged(),
            _ when this == RenewalSuccessRecreated => onRenewalSuccessRecreated(),
            _ when this == RenewalFailureRecreated => onRenewalFailureRecreated(),
            _ when this == PaymentSuccessRecreated => onPaymentSuccessRecreated(),
            _ when this == PaymentFailureRecreated => onPaymentFailureRecreated(),
            _ when this == SubscriptionDeletion => onSubscriptionDeletion(),
            _ when this == SubscriptionGroupBankAccountUpdate => onSubscriptionGroupBankAccountUpdate(),
            _ when this == SubscriptionPaypalAccountUpdate => onSubscriptionPaypalAccountUpdate(),
            _ when this == SubscriptionGroupPaypalAccountUpdate => onSubscriptionGroupPaypalAccountUpdate(),
            _ when this == SubscriptionCustomerChange => onSubscriptionCustomerChange(),
            _ when this == AccountTransactionChanged => onAccountTransactionChanged(),
            _ when this == GoCardlessPaymentPaidOut => onGoCardlessPaymentPaidOut(),
            _ when this == GoCardlessPaymentRejected => onGoCardlessPaymentRejected(),
            _ when this == GoCardlessPaymentPending => onGoCardlessPaymentPending(),
            _ when this == StripeDirectDebitPaymentPaidOut => onStripeDirectDebitPaymentPaidOut(),
            _ when this == StripeDirectDebitPaymentRejected => onStripeDirectDebitPaymentRejected(),
            _ when this == StripeDirectDebitPaymentPending => onStripeDirectDebitPaymentPending(),
            _ when this == MaxioPaymentsDirectDebitPaymentPaidOut => onMaxioPaymentsDirectDebitPaymentPaidOut(),
            _ when this == MaxioPaymentsDirectDebitPaymentRejected => onMaxioPaymentsDirectDebitPaymentRejected(),
            _ when this == MaxioPaymentsDirectDebitPaymentPending => onMaxioPaymentsDirectDebitPaymentPending(),
            _ when this == InvoiceInCollectionsCanceled => onInvoiceInCollectionsCanceled(),
            _ when this == SubscriptionAddedToGroup => onSubscriptionAddedToGroup(),
            _ when this == SubscriptionRemovedFromGroup => onSubscriptionRemovedFromGroup(),
            _ when this == ChargebackOpened => onChargebackOpened(),
            _ when this == ChargebackLost => onChargebackLost(),
            _ when this == ChargebackAccepted => onChargebackAccepted(),
            _ when this == ChargebackClosed => onChargebackClosed(),
            _ when this == ChargebackWon => onChargebackWon(),
            _ when this == PaymentCollectionMethodChanged => onPaymentCollectionMethodChanged(),
            _ when this == ComponentBillingDateChanged => onComponentBillingDateChanged(),
            _ when this == ChjsTokenizationFailure => onChjsTokenizationFailure(),
            _ when this == ChjsTokenizationSuccess => onChjsTokenizationSuccess(),
            _ when this == SubscriptionTermRenewalScheduled => onSubscriptionTermRenewalScheduled(),
            _ when this == SubscriptionTermRenewalPending => onSubscriptionTermRenewalPending(),
            _ when this == SubscriptionTermRenewalActivated => onSubscriptionTermRenewalActivated(),
            _ when this == SubscriptionTermRenewalRemoved => onSubscriptionTermRenewalRemoved(),
            _ => otherwise(Value)
        };

    public void Match(Action onPaymentSuccess,
        Action onPaymentFailure,
        Action onSignupSuccess,
        Action onSignupFailure,
        Action onDelayedSignupCreationSuccess,
        Action onDelayedSignupCreationFailure,
        Action onBillingDateChange,
        Action onExpirationDateChange,
        Action onRenewalSuccess,
        Action onRenewalFailure,
        Action onSubscriptionStateChange,
        Action onSubscriptionProductChange,
        Action onSubscriptionProductChangeScheduled,
        Action onPendingCancellationChange,
        Action onExpiringCard,
        Action onCustomerUpdate,
        Action onCustomerCreate,
        Action onCustomerDelete,
        Action onComponentAllocationChange,
        Action onMeteredUsage,
        Action onPrepaidUsage,
        Action onUpgradeDowngradeSuccess,
        Action onUpgradeDowngradeFailure,
        Action onStatementClosed,
        Action onStatementSettled,
        Action onSubscriptionCardUpdate,
        Action onSubscriptionGroupCardUpdate,
        Action onSubscriptionBankAccountUpdate,
        Action onRefundSuccess,
        Action onRefundFailure,
        Action onUpcomingRenewalNotice,
        Action onTrialEndNotice,
        Action onDunningStepReached,
        Action onInvoiceIssued,
        Action onInvoicePending,
        Action onPrepaidSubscriptionBalanceChanged,
        Action onSubscriptionGroupSignupSuccess,
        Action onSubscriptionGroupSignupFailure,
        Action onDirectDebitPaymentPaidOut,
        Action onDirectDebitPaymentRejected,
        Action onDirectDebitPaymentPending,
        Action onPendingPaymentCreated,
        Action onPendingPaymentFailed,
        Action onPendingPaymentCompleted,
        Action onProformaInvoiceIssued,
        Action onSubscriptionPrepaymentAccountBalanceChanged,
        Action onSubscriptionServiceCreditAccountBalanceChanged,
        Action onCustomFieldValueChange,
        Action onItemPricePointChanged,
        Action onRenewalSuccessRecreated,
        Action onRenewalFailureRecreated,
        Action onPaymentSuccessRecreated,
        Action onPaymentFailureRecreated,
        Action onSubscriptionDeletion,
        Action onSubscriptionGroupBankAccountUpdate,
        Action onSubscriptionPaypalAccountUpdate,
        Action onSubscriptionGroupPaypalAccountUpdate,
        Action onSubscriptionCustomerChange,
        Action onAccountTransactionChanged,
        Action onGoCardlessPaymentPaidOut,
        Action onGoCardlessPaymentRejected,
        Action onGoCardlessPaymentPending,
        Action onStripeDirectDebitPaymentPaidOut,
        Action onStripeDirectDebitPaymentRejected,
        Action onStripeDirectDebitPaymentPending,
        Action onMaxioPaymentsDirectDebitPaymentPaidOut,
        Action onMaxioPaymentsDirectDebitPaymentRejected,
        Action onMaxioPaymentsDirectDebitPaymentPending,
        Action onInvoiceInCollectionsCanceled,
        Action onSubscriptionAddedToGroup,
        Action onSubscriptionRemovedFromGroup,
        Action onChargebackOpened,
        Action onChargebackLost,
        Action onChargebackAccepted,
        Action onChargebackClosed,
        Action onChargebackWon,
        Action onPaymentCollectionMethodChanged,
        Action onComponentBillingDateChanged,
        Action onChjsTokenizationFailure,
        Action onChjsTokenizationSuccess,
        Action onSubscriptionTermRenewalScheduled,
        Action onSubscriptionTermRenewalPending,
        Action onSubscriptionTermRenewalActivated,
        Action onSubscriptionTermRenewalRemoved,
        Action<string> otherwise)
    {
        if (this == PaymentSuccess) onPaymentSuccess();
        else if (this == PaymentFailure) onPaymentFailure();
        else if (this == SignupSuccess) onSignupSuccess();
        else if (this == SignupFailure) onSignupFailure();
        else if (this == DelayedSignupCreationSuccess) onDelayedSignupCreationSuccess();
        else if (this == DelayedSignupCreationFailure) onDelayedSignupCreationFailure();
        else if (this == BillingDateChange) onBillingDateChange();
        else if (this == ExpirationDateChange) onExpirationDateChange();
        else if (this == RenewalSuccess) onRenewalSuccess();
        else if (this == RenewalFailure) onRenewalFailure();
        else if (this == SubscriptionStateChange) onSubscriptionStateChange();
        else if (this == SubscriptionProductChange) onSubscriptionProductChange();
        else if (this == SubscriptionProductChangeScheduled) onSubscriptionProductChangeScheduled();
        else if (this == PendingCancellationChange) onPendingCancellationChange();
        else if (this == ExpiringCard) onExpiringCard();
        else if (this == CustomerUpdate) onCustomerUpdate();
        else if (this == CustomerCreate) onCustomerCreate();
        else if (this == CustomerDelete) onCustomerDelete();
        else if (this == ComponentAllocationChange) onComponentAllocationChange();
        else if (this == MeteredUsage) onMeteredUsage();
        else if (this == PrepaidUsage) onPrepaidUsage();
        else if (this == UpgradeDowngradeSuccess) onUpgradeDowngradeSuccess();
        else if (this == UpgradeDowngradeFailure) onUpgradeDowngradeFailure();
        else if (this == StatementClosed) onStatementClosed();
        else if (this == StatementSettled) onStatementSettled();
        else if (this == SubscriptionCardUpdate) onSubscriptionCardUpdate();
        else if (this == SubscriptionGroupCardUpdate) onSubscriptionGroupCardUpdate();
        else if (this == SubscriptionBankAccountUpdate) onSubscriptionBankAccountUpdate();
        else if (this == RefundSuccess) onRefundSuccess();
        else if (this == RefundFailure) onRefundFailure();
        else if (this == UpcomingRenewalNotice) onUpcomingRenewalNotice();
        else if (this == TrialEndNotice) onTrialEndNotice();
        else if (this == DunningStepReached) onDunningStepReached();
        else if (this == InvoiceIssued) onInvoiceIssued();
        else if (this == InvoicePending) onInvoicePending();
        else if (this == PrepaidSubscriptionBalanceChanged) onPrepaidSubscriptionBalanceChanged();
        else if (this == SubscriptionGroupSignupSuccess) onSubscriptionGroupSignupSuccess();
        else if (this == SubscriptionGroupSignupFailure) onSubscriptionGroupSignupFailure();
        else if (this == DirectDebitPaymentPaidOut) onDirectDebitPaymentPaidOut();
        else if (this == DirectDebitPaymentRejected) onDirectDebitPaymentRejected();
        else if (this == DirectDebitPaymentPending) onDirectDebitPaymentPending();
        else if (this == PendingPaymentCreated) onPendingPaymentCreated();
        else if (this == PendingPaymentFailed) onPendingPaymentFailed();
        else if (this == PendingPaymentCompleted) onPendingPaymentCompleted();
        else if (this == ProformaInvoiceIssued) onProformaInvoiceIssued();
        else if (this == SubscriptionPrepaymentAccountBalanceChanged) onSubscriptionPrepaymentAccountBalanceChanged();
        else if (this ==
            SubscriptionServiceCreditAccountBalanceChanged) onSubscriptionServiceCreditAccountBalanceChanged();
        else if (this == CustomFieldValueChange) onCustomFieldValueChange();
        else if (this == ItemPricePointChanged) onItemPricePointChanged();
        else if (this == RenewalSuccessRecreated) onRenewalSuccessRecreated();
        else if (this == RenewalFailureRecreated) onRenewalFailureRecreated();
        else if (this == PaymentSuccessRecreated) onPaymentSuccessRecreated();
        else if (this == PaymentFailureRecreated) onPaymentFailureRecreated();
        else if (this == SubscriptionDeletion) onSubscriptionDeletion();
        else if (this == SubscriptionGroupBankAccountUpdate) onSubscriptionGroupBankAccountUpdate();
        else if (this == SubscriptionPaypalAccountUpdate) onSubscriptionPaypalAccountUpdate();
        else if (this == SubscriptionGroupPaypalAccountUpdate) onSubscriptionGroupPaypalAccountUpdate();
        else if (this == SubscriptionCustomerChange) onSubscriptionCustomerChange();
        else if (this == AccountTransactionChanged) onAccountTransactionChanged();
        else if (this == GoCardlessPaymentPaidOut) onGoCardlessPaymentPaidOut();
        else if (this == GoCardlessPaymentRejected) onGoCardlessPaymentRejected();
        else if (this == GoCardlessPaymentPending) onGoCardlessPaymentPending();
        else if (this == StripeDirectDebitPaymentPaidOut) onStripeDirectDebitPaymentPaidOut();
        else if (this == StripeDirectDebitPaymentRejected) onStripeDirectDebitPaymentRejected();
        else if (this == StripeDirectDebitPaymentPending) onStripeDirectDebitPaymentPending();
        else if (this == MaxioPaymentsDirectDebitPaymentPaidOut) onMaxioPaymentsDirectDebitPaymentPaidOut();
        else if (this == MaxioPaymentsDirectDebitPaymentRejected) onMaxioPaymentsDirectDebitPaymentRejected();
        else if (this == MaxioPaymentsDirectDebitPaymentPending) onMaxioPaymentsDirectDebitPaymentPending();
        else if (this == InvoiceInCollectionsCanceled) onInvoiceInCollectionsCanceled();
        else if (this == SubscriptionAddedToGroup) onSubscriptionAddedToGroup();
        else if (this == SubscriptionRemovedFromGroup) onSubscriptionRemovedFromGroup();
        else if (this == ChargebackOpened) onChargebackOpened();
        else if (this == ChargebackLost) onChargebackLost();
        else if (this == ChargebackAccepted) onChargebackAccepted();
        else if (this == ChargebackClosed) onChargebackClosed();
        else if (this == ChargebackWon) onChargebackWon();
        else if (this == PaymentCollectionMethodChanged) onPaymentCollectionMethodChanged();
        else if (this == ComponentBillingDateChanged) onComponentBillingDateChanged();
        else if (this == ChjsTokenizationFailure) onChjsTokenizationFailure();
        else if (this == ChjsTokenizationSuccess) onChjsTokenizationSuccess();
        else if (this == SubscriptionTermRenewalScheduled) onSubscriptionTermRenewalScheduled();
        else if (this == SubscriptionTermRenewalPending) onSubscriptionTermRenewalPending();
        else if (this == SubscriptionTermRenewalActivated) onSubscriptionTermRenewalActivated();
        else if (this == SubscriptionTermRenewalRemoved) onSubscriptionTermRenewalRemoved();
        else otherwise(Value);
    }
}
