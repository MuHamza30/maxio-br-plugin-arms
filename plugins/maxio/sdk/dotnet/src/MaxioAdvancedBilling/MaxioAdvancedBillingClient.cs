using System.Net.Http;
using MaxioAdvancedBilling.Api;
using MaxioAdvancedBilling.Core;
using MaxioAdvancedBilling.Core.Configuration;
using MaxioAdvancedBilling.Core.Logging;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling;

/// <summary>
/// Maxio Advanced Billing (formerly Chargify) provides an HTTP-based API that conforms to the principles of REST.
/// One of the many reasons to use Advanced Billing is the immense feature set and <see href="page:development-tools/client-libraries">client libraries</see>.
/// The Maxio API returns JSON responses as the primary and recommended format, but XML is also provided as a backwards compatible option for merchants who require it.
/// <para>
/// ## Steps to make your first Maxio Advanced Billing API call
/// </para>
/// <list type="number">
///   <item><description><see href="https://app.chargify.com/signup/maxio-billing-sandbox">Sign-up</see> or <see href="https://app.chargify.com/login.html">log-in</see> to your <see href="https://maxio.zendesk.com/hc/en-us/articles/24250712113165-Testing-Overview">test site</see> account.</description></item>
///   <item><description><see href="https://maxio.zendesk.com/hc/en-us/articles/24294819360525-API-Keys">Setup authentication</see> credentials.</description></item>
///   <item><description><see href="page:development-tools/client-libraries#make-your-first-maxio-advanced-billing-api-request">Submit an API request and verify the response</see>.</description></item>
///   <item><description>Test the Advanced Billing <see href="https://www.maxio.com/integrations">integrations</see>.</description></item>
/// </list>
/// <para>
/// Next, you can explore <see href="page:introduction/authentication">authentication methods</see>, <see href="page:introduction/basic-concepts/connected-sites">basic concepts</see> for interacting with Advanced Billing via the API, and the entire set of <see href="https://docs.maxio.com/hc/en-us">application-based documentation</see> to aid in your discovery of the product.
/// </para>
/// <para>
/// ### Request Example
/// </para>
/// <para>
/// The following example uses the curl command-line tool to make an API request.
/// </para>
/// <para>
/// <b>Request</b>
/// </para>
/// <para>
///     curl -u &lt;api_key&gt;:x -H Accept:application/json -H Content-Type:application/json https://acme.chargify.com/subscriptions.json
/// </para>
/// </summary>
public sealed class MaxioAdvancedBillingClient
{
    private readonly RawClient _rawClient;
    private readonly Server _server;
    private readonly AuthSchemes _auth;

    public MaxioAdvancedBillingClient(HttpClient httpClient, MaxioAdvancedBillingClientOptions options)
    {
        _server = new Server(options.Environment, options.Server);
        var queryParameterFactory = new QueryParameterFactory([]);
        var templateParamsFactory = new TemplateParamsFactory([]);
        var urlFactory = new UriFactory(queryParameterFactory, templateParamsFactory);
        var httpStatusPolicy = new HttpStatusPolicy([]);
        var headersFactory = new HeadersFactory([
            new HeaderParam("User-Agent", "MaxioAdvancedBillingClient/1.0 CSharp"),
            new HeaderParam("X-APIMatic-Lang", "CSharp"),
            new HeaderParam("X-APIMatic-Package-Version", "1.0"),
            new HeaderParam("X-APIMatic-Gen-Version", "4.0.0"),
            new HeaderParam("X-APIMatic-OS", RuntimeEnvironment.Os),
            new HeaderParam("X-APIMatic-Runtime", RuntimeEnvironment.Runtime),
        ]);
        var resiliencePipelineFactory = new ResiliencePipelineFactory(options.Retry, options.TimeProvider);
        var httpLogger = new HttpLogger(
            options.Logging.WithEnvironmentDefaults("MAXIO_ADVANCED_BILLING_LOG"),
            "MaxioAdvancedBillingClient",
            options.TimeProvider);
        var responseContexts = new ResponseContextFactory(options.TimeProvider, options.StreamReadTimeout);
        _rawClient =
            new RawClient(
                httpClient,
                urlFactory,
                httpStatusPolicy,
                headersFactory,
                resiliencePipelineFactory,
                httpLogger,
                options.Hooks,
                responseContexts);
        _auth = new AuthSchemes(options);
    }

    /// <summary>
    /// API Exports allow you to generate and download bulk exports of subscriptions, invoices, and proforma invoices as asynchronous batch jobs, useful for reporting or migrating data outside the standard paginated list endpoints. For more information, see <see href="https://docs.maxio.com/insights/data-export/field-references/billing-export-fields-reference#invoice-line-items-fields">Billing Export Fields Reference</see>.
    /// </summary>
    public ApiExports ApiExports => field ??= new ApiExports(_rawClient, _server, _auth);

    /// <summary>
    /// Use the Advance Invoice feature to issue, retrieve, or void an invoice for a subscription ahead of its normal billing schedule, collecting an outstanding balance before the next scheduled renewal. For more information, see <see href="https://docs.maxio.com/billing/invoicing/advance-invoices/issue-advance-invoices-for-subscription-renewals">Issue Advance Invoices for Subscription Renewals</see>.
    /// </summary>
    public AdvanceInvoice AdvanceInvoice => field ??= new AdvanceInvoice(_rawClient, _server, _auth);

    /// <summary>
    /// The Billing Portal object allows you to enable, disable, and manage customer access to the self-service Billing Portal, where customers can view invoices and manage their own payment details. For more information, see <see href="https://docs.maxio.com/billing/billing-portal/overview/understand-the-billing-portal">Understand the Billing Portal</see>.
    /// </summary>
    public BillingPortal BillingPortal => field ??= new BillingPortal(_rawClient, _server, _auth);

    /// <summary>
    /// The Component Feature object attaches a Feature Template to a Component as a feature catalog item with a concrete value, granting that feature to subscribers through the component.
    /// <para>
    /// A component feature applies to the whole component by default, or can be scoped to a single component price point as an override. For more information, see <see href="https://docs.maxio.com/billing/catalog/entitlements/configure-entitlements">Configure Entitlements</see>.
    /// </para>
    /// </summary>
    public ComponentFeatures ComponentFeatures => field ??= new ComponentFeatures(_rawClient, _server, _auth);

    /// <summary>
    /// The Component Price Point object defines a specific pricing configuration for a Component, including the price point's rate structure and billing interval.
    /// <para>
    /// A single Component can have multiple price points, allowing different subscriptions to be billed for the same component under different terms.
    /// </para>
    /// </summary>
    public ComponentPricePoints ComponentPricePoints => field ??= new ComponentPricePoints(_rawClient, _server, _auth);

    /// <summary>
    /// The Component object represents an add-on item, such as a metered usage charge, quantity-based feature, or on/off toggle, that can be added to a subscription alongside the subscription's base Product. With the new catalog experience, the UI labels components as Products, while the API keeps the component name. See <see href="https://docs.maxio.com/billing/catalog/overview/using-the-new-product-catalog#catalog-terminology">Catalog terminology</see>.
    /// <para>
    /// Components belong to a Product Family and are billed according to their pricing scheme and kind. Components can also grant features to subscribers. See the <b>Create Component Feature Catalog Item</b> endpoint for details. For more information, see <see href="https://docs.maxio.com/billing/catalog/products/understand-products">Understand Products</see> and <see href="https://docs.maxio.com/billing/catalog/products/use-product-pricing-schemes#hybrid-pricing">Hybrid pricing</see>.
    /// </para>
    /// </summary>
    public Components Components => field ??= new Components(_rawClient, _server, _auth);

    /// <summary>
    /// The Coupon object defines a discount, either a percentage or a fixed amount, that can be applied to a subscription to reduce the subscription's billing amount.
    /// <para>
    /// Coupons can be restricted to a specific Product Family, limited to a date range, and configured to apply once or recur over a set duration. For more information, see <see href="https://docs.maxio.com/billing/catalog/coupons/understand-coupons">Understand Coupons</see>.
    /// </para>
    /// </summary>
    public Coupons Coupons => field ??= new Coupons(_rawClient, _server, _auth);

    /// <summary>
    /// Custom Fields let you define Metafields for a resource type and manage the Metadata values attached to individual records, so you can capture and store additional data beyond built-in system fields. For more information, see <see href="https://docs.maxio.com/configuration/advanced-billing/custom-fields/understand-custom-fields">Understand Custom Fields</see>.
    /// </summary>
    public CustomFields CustomFields => field ??= new CustomFields(_rawClient, _server, _auth);

    /// <summary>
    /// The Customer object represents the person or organization being billed and is the anchor that subscriptions, payment profiles, and invoices are all attached to.
    /// <para>
    /// A customer record stores contact and billing details (name, address, tax exemption status, locale) along with references to related records, such as the customer's default subscription group. For more information, see <see href="https://docs.maxio.com/billing/subscriptions/customers/understand-customers-for-subscription-management">Understand Customers for Subscription Management</see>.
    /// </para>
    /// </summary>
    public Customers Customers => field ??= new Customers(_rawClient, _server, _auth);

    /// <summary>
    /// Entitlements report what a subscription is allowed to do, and how much of it, based on the features granted by the subscription's products and components.
    /// <para>
    /// When more than one product or component grants the same feature, the values are aggregated into one entry per feature key and periodicity. For more information, see <see href="https://docs.maxio.com/billing/catalog/entitlements/read-subscription-entitlements-with-the-api">Read Subscription Entitlements with the API</see>.
    /// </para>
    /// </summary>
    public Entitlements Entitlements => field ??= new Entitlements(_rawClient, _server, _auth);

    /// <summary>
    /// The Event object records a discrete occurrence within your account, such as a payment, renewal, or subscription state change.
    /// <para>
    /// The key identifies which type of event occurred, and event_specific_data carries a payload whose shape depends on that key.
    /// </para>
    /// </summary>
    public Events Events => field ??= new Events(_rawClient, _server, _auth);

    /// <summary>
    /// The Segment object defines a distinct pricing variation within an event-based billing metric, based on one or more property values, such as a specific country or usage category.
    /// <para>
    /// Each segment carries its own set of prices, letting a single metric charge different rates depending on which segment an event falls into.
    /// </para>
    /// </summary>
    public EventsBasedBillingSegments EventsBasedBillingSegments =>
        field ??= new EventsBasedBillingSegments(_rawClient, _server, _auth);

    /// <summary>
    /// The Feature Template object defines a feature once at the site level, such as Single Sign-On, a monthly API call limit, or a support tier.
    /// <para>
    /// A feature template is not billable on its own. Attach the template to products or components to grant the feature to subscribers. The kind (access_right, usage_limit, or service_right) determines how the feature's value behaves. For more information, see <see href="https://docs.maxio.com/billing/catalog/entitlements/configure-entitlements">Configure Entitlements</see>.
    /// </para>
    /// </summary>
    public FeatureTemplates FeatureTemplates => field ??= new FeatureTemplates(_rawClient, _server, _auth);

    /// <summary>
    /// The Insights endpoints provide aggregate site-level statistics, such as customer and subscription counts, giving a high-level snapshot of account activity.
    /// </summary>
    public Insights Insights => field ??= new Insights(_rawClient, _server, _auth);

    /// <summary>
    /// The Invoice object is the billing document generated for a subscription, itemizing charges, taxes, and payments, and tracking amounts owed.
    /// <para>
    /// The role indicates what triggered the invoice (signup, renewal, usage, proration, etc.), and the status and due_date reflect where an invoice stands in the payment lifecycle. For more information, see <see href="https://docs.maxio.com/billing/invoicing/overview/get-started-with-invoicing">Get Started with Invoicing</see>.
    /// </para>
    /// </summary>
    public Invoices Invoices => field ??= new Invoices(_rawClient, _server, _auth);

    /// <summary>
    /// The Offer object bundles a specific Product, Price Point, and any associated discounts into a single pre-configured signup option that customers can subscribe to directly.
    /// <para>
    /// Offers are commonly used to power self-service signup pages without requiring each pricing combination to be built manually. For more information, see <see href="https://docs.maxio.com/billing/catalog/offers/understand-offers">Understand Offers</see>.
    /// </para>
    /// </summary>
    public Offers Offers => field ??= new Offers(_rawClient, _server, _auth);

    /// <summary>
    /// The Payment Profile object represents a stored payment method on a customer's account. For more information, see <see href="https://docs.maxio.com/getting-started/maxio-platform-basics/advanced-billing-overview-and-resources/subscription-summary-tabs/managing-payment-methods">Managing Payment Methods</see>.
    /// </summary>
    public PaymentProfiles PaymentProfiles => field ??= new PaymentProfiles(_rawClient, _server, _auth);

    /// <summary>
    /// The Product Family object groups related Products together for organizational purposes, such as products belonging to a single line of business, each sharing a family-level handle and accounting code. For more information, see <see href="https://docs.maxio.com/billing/catalog/plans/organize-product-families">Organize Product Families</see>.
    /// </summary>
    public ProductFamilies ProductFamilies => field ??= new ProductFamilies(_rawClient, _server, _auth);

    /// <summary>
    /// The Product Feature object attaches a Feature Template to a Product as a feature catalog item with a concrete value, granting that feature to subscribers through the product.
    /// <para>
    /// A product feature applies to the whole product by default, or can be scoped to a single product price point as an override. For more information, see <see href="https://docs.maxio.com/billing/catalog/entitlements/configure-entitlements">Configure Entitlements</see>.
    /// </para>
    /// </summary>
    public ProductFeatures ProductFeatures => field ??= new ProductFeatures(_rawClient, _server, _auth);

    /// <summary>
    /// The Product Price Point object defines a specific pricing configuration for a Product, including price, billing interval, and trial terms.
    /// <para>
    /// A single Product can have multiple price points, letting you offer different rates or terms for the same product without creating separate products. For more information, see <see href="https://docs.maxio.com/billing/catalog/plans/setting-up-plan-price-points">Setting Up Plan Price Points</see>.
    /// </para>
    /// </summary>
    public ProductPricePoints ProductPricePoints => field ??= new ProductPricePoints(_rawClient, _server, _auth);

    /// <summary>
    /// The Product object defines what you sell, including default pricing, billing interval, and trial configuration. With the new catalog experience, the UI labels products as Plans, while the API keeps the product name. See <see href="https://docs.maxio.com/billing/catalog/overview/using-the-new-product-catalog#catalog-terminology">Catalog terminology</see>.
    /// <para>
    /// Products are grouped under a Product Family and are the base type a customer subscribes to. Products can also grant features to subscribers. See the <b>Create Product Feature Catalog Item</b> endpoint for details. For more information, see <see href="https://docs.maxio.com/billing/catalog/plans/understand-plans">Understand Plans</see>.
    /// </para>
    /// </summary>
    public Products Products => field ??= new Products(_rawClient, _server, _auth);

    /// <summary>
    /// The Proforma Invoice object is a preview of an upcoming invoice, showing the charges a customer will be billed before the actual invoice is issued. A proforma invoice is not a finalized billing document and is commonly used to give customers advance visibility into what they owe. For more information, see <see href="https://docs.maxio.com/billing/invoicing/proforma-invoices/understand-proforma-invoices">Understand Proforma Invoices</see>.
    /// </summary>
    public ProformaInvoices ProformaInvoices => field ??= new ProformaInvoices(_rawClient, _server, _auth);

    /// <summary>
    /// The Reason Code object defines a churn reason that can be presented to customers when they cancel a subscription, such as "too expensive" or "switching providers."
    /// <para>
    /// Reason codes give merchants visibility into why subscribers are canceling. For more information, see <see href="https://docs.maxio.com/configuration/advanced-billing/settings/churn-reason-codes">Configure Churn Reason Codes</see>.
    /// </para>
    /// </summary>
    public ReasonCodes ReasonCodes => field ??= new ReasonCodes(_rawClient, _server, _auth);

    /// <summary>
    /// The Referral Code object represents a code attached to a subscription, used to track and validate referrals as part of a referral program. For more information, see <see href="https://docs.maxio.com/configuration/advanced-billing/understand-referrals">Understand Referrals</see>.
    /// </summary>
    public ReferralCodes ReferralCodes => field ??= new ReferralCodes(_rawClient, _server, _auth);

    /// <summary>
    /// Sales Commissions allow sellers with financial access to retrieve commission data for a site with Advanced Analytics enabled.
    /// <para>
    /// These endpoints use a different, user-level authentication method than the rest of the API, since commission data is tied to the seller rather than an individual site.
    /// </para>
    /// </summary>
    public SalesCommissions SalesCommissions => field ??= new SalesCommissions(_rawClient, _server, _auth);

    /// <summary>
    /// The Site object represents a single billing account and holds site-wide configuration, such as default currency, tax settings, net terms, and which billing features are enabled. For more information, see <see href="https://docs.maxio.com/configuration/advanced-billing/sites/sites-overview">Understand Advanced Billing Sites</see>.
    /// </summary>
    public Sites Sites => field ??= new Sites(_rawClient, _server, _auth);

    /// <summary>
    /// Use the Subscription Components object to allocate, adjust, and report usage for the components attached to a subscription, including quantity-based, metered, and event-based components. For more information, see <see href="https://docs.maxio.com/getting-started/maxio-platform-basics/advanced-billing-overview-and-resources/subscription-summary-tabs/managing-components-and-usage">Managing Components and Usage</see>.
    /// </summary>
    public SubscriptionComponents SubscriptionComponents =>
        field ??= new SubscriptionComponents(_rawClient, _server, _auth);

    /// <summary>
    /// The Subscription Group Invoice Account object lets you manage prepayments and service credits for the shared invoice account of a Subscription Group, applying at the group level rather than to an individual subscription.
    /// </summary>
    public SubscriptionGroupInvoiceAccount SubscriptionGroupInvoiceAccount =>
        field ??= new SubscriptionGroupInvoiceAccount(_rawClient, _server, _auth);

    /// <summary>
    /// Use the Subscription Group Status object to manage the lifecycle of a Subscription Group as a whole, such as canceling every subscription within the group together.
    /// </summary>
    public SubscriptionGroupStatus SubscriptionGroupStatus =>
        field ??= new SubscriptionGroupStatus(_rawClient, _server, _auth);

    /// <summary>
    /// The Subscription Group object links multiple subscriptions for a single customer under one shared payment profile and collection method, so you can consolidate billing instead of managing payment details separately for each subscription. For more information, see <see href="https://docs.maxio.com/billing/subscriptions/configuration/subscription-groups/understand-subscription-groups">Understand Subscription Groups</see>.
    /// </summary>
    public SubscriptionGroups SubscriptionGroups => field ??= new SubscriptionGroups(_rawClient, _server, _auth);

    /// <summary>
    /// The Subscription Invoice Account object lets you manage a subscription's account balance, including prepayments, service credits, and refunds. For more information, see <see href="https://docs.maxio.com/getting-started/maxio-platform-basics/advanced-billing-overview-and-resources/subscription-summary-tabs/recording-prepayments">Recording Prepayments</see> and <see href="https://docs.maxio.com/billing/subscriptions/management/subscription-actions/give-deduct-and-apply-service-credits">Give, Deduct, and Apply Service Credits</see>.
    /// </summary>
    public SubscriptionInvoiceAccount SubscriptionInvoiceAccount =>
        field ??= new SubscriptionInvoiceAccount(_rawClient, _server, _auth);

    /// <summary>
    /// The Subscription Note object stores an internal note attached to a subscription, used to record context, decisions, or history for your team.
    /// <para>
    /// Notes are never exposed to the customer; they exist purely for internal reference.
    /// </para>
    /// </summary>
    public SubscriptionNotes SubscriptionNotes => field ??= new SubscriptionNotes(_rawClient, _server, _auth);

    /// <summary>
    /// Use the Subscription Products object to migrate a subscription from one product or price point to another, including previewing the effects of a migration before committing to it. For more information, see <see href="https://docs.maxio.com/billing/subscriptions/management/subscription-actions/upgrade-or-downgrade-a-subscription">Upgrade or Downgrade a Subscription</see>.
    /// </summary>
    public SubscriptionProducts SubscriptionProducts => field ??= new SubscriptionProducts(_rawClient, _server, _auth);

    /// <summary>
    /// The Subscription Renewals object lets you schedule, preview, and manage a subscription's upcoming renewal, including locking in specific pricing or canceling a scheduled renewal before it takes effect. For more information, see <see href="https://docs.maxio.com/billing/subscriptions/configuration/term-renewal/scheduled-renewal">Understand Scheduled Renewal</see>.
    /// </summary>
    public SubscriptionRenewals SubscriptionRenewals => field ??= new SubscriptionRenewals(_rawClient, _server, _auth);

    /// <summary>
    /// Subscription Status allows you to manage a subscription's lifecycle, including retrying a failed payment, canceling, pausing, resuming, and reactivating a subscription. For more information, see <see href="https://docs.maxio.com/billing/subscriptions/management/subscription-states/understand-subscription-states">Understand Subscription States</see>.
    /// </summary>
    public SubscriptionStatus SubscriptionStatus => field ??= new SubscriptionStatus(_rawClient, _server, _auth);

    /// <summary>
    /// Subscriptions connect a customer to a product and drive all recurring billing for that relationship, including invoice generation, billing start dates, and pricing structure across plan-based and planless configurations. For more information, see <see href="https://docs.maxio.com/billing/subscriptions/overview/understand-subscriptions">Understand Subscriptions</see>.
    /// </summary>
    public Subscriptions Subscriptions => field ??= new Subscriptions(_rawClient, _server, _auth);

    /// <summary>
    /// Wallet Adjustments post a signed correction against a specific grant, or against an overdraft, on a wallet's ledger. Adjustments are intended for support and operations corrections, and each adjustment requires a reason that is recorded on the ledger. For more information, see <see href="https://docs.maxio.com/billing/catalog/wallets/understand-wallets">Understand Wallets</see>.
    /// </summary>
    public WalletAdjustments WalletAdjustments => field ??= new WalletAdjustments(_rawClient, _server, _auth);

    /// <summary>
    /// The Wallet Product object is a billing product created specifically to denominate wallets and to price wallet purchases. A Wallet Product does not belong to a product family, and the Wallet Product behind a wallet cannot be changed after the wallet is created. For more information, see <see href="https://docs.maxio.com/billing/catalog/wallets/create-and-manage-wallet-products">Create and Manage Wallet Products</see>.
    /// </summary>
    public WalletProducts WalletProducts => field ??= new WalletProducts(_rawClient, _server, _auth);

    /// <summary>
    /// Wallet Purchases let a subscription buy additional prepaid units for a wallet, charged according to the subscription's collection method. A purchase can be previewed to quote the price before anything is charged. For more information, see <see href="https://docs.maxio.com/billing/catalog/wallets/understand-wallets">Understand Wallets</see>.
    /// </summary>
    public WalletPurchases WalletPurchases => field ??= new WalletPurchases(_rawClient, _server, _auth);

    /// <summary>
    /// Use Wallet Transactions to fund a wallet with a one-time grant, consume units with a direct debit, and read the wallet's append-only ledger. For more information, see <see href="https://docs.maxio.com/billing/catalog/wallets/understand-wallets">Understand Wallets</see>.
    /// </summary>
    public WalletTransactions WalletTransactions => field ??= new WalletTransactions(_rawClient, _server, _auth);

    /// <summary>
    /// The Wallet object is a balance, owned by a subscription and denominated in the unit of a Wallet Product, that you can create, fund, drain, and read through the API.
    /// <para>
    /// Wallets suit prepaid allowances, credits, and consumption pools that don't map cleanly onto the regular usage-billing cycle. Every balance change is recorded on the wallet's append-only ledger. For more information, see <see href="https://docs.maxio.com/billing/catalog/wallets/understand-wallets">Understand Wallets</see>.
    /// </para>
    /// </summary>
    public Wallets Wallets => field ??= new Wallets(_rawClient, _server, _auth);

    /// <summary>
    /// The Webhook object represents a single notification sent to your configured endpoint when an event occurs.
    /// <para>
    /// A webhook tracks delivery status, including when it was last sent, whether the merchant endpoint accepted it, and any error from a failed attempt. For more information, see <see href="https://docs.maxio.com/configuration/advanced-billing/webhooks/understand-webhooks-in-advanced-billing">Understand Webhooks in Advanced Billing</see>.
    /// </para>
    /// </summary>
    public WebhooksApi WebhooksApi => field ??= new WebhooksApi(_rawClient, _server, _auth);
}
