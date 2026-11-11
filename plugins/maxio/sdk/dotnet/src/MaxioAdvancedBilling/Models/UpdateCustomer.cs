using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models.Enums;

namespace MaxioAdvancedBilling.Models;

public record UpdateCustomer
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("first_name")]
    public string? FirstName { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("last_name")]
    public string? LastName { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("email")]
    public string? Email { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("cc_emails")]
    public string? CcEmails { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("organization")]
    public string? Organization { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("reference")]
    public string? Reference { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("address")]
    public string? Address { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("address_2")]
    public string? Address2 { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("city")]
    public string? City { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("state")]
    public string? State { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("zip")]
    public string? Zip { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("country")]
    public string? Country { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("phone")]
    public string? Phone { get; init; }

    /// <summary>
    /// Set a specific language on a customer record.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("locale")]
    public string? Locale { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("vat_number")]
    public string? VatNumber { get; init; }

    /// <summary>
    /// The two-letter ISO 3166-1 country code that qualifies the customer's tax ID. Required when <c>entity_identifier_kind</c> is <c>vat_eu</c> or <c>national_tax</c>, and used to derive the kind when only the legacy <c>vat_number</c> is sent.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("vat_country")]
    public string? VatCountry { get; init; }

    /// <summary>
    /// The kind of tax or business identifier held by the customer:
    /// - <c>vat_eu</c>: an EU VAT number. Requires <c>vat_country</c> to be an EU member state code or <c>GB</c>.
    /// - <c>national_tax</c>: a national tax ID registered outside the EU. Requires <c>vat_country</c> to be one of <c>AL</c>, <c>AM</c>, <c>AR</c>, <c>AU</c>, <c>BR</c>, <c>CA</c>, <c>CH</c>, <c>DZ</c>, <c>IN</c>, <c>MX</c>, <c>NO</c>, <c>NZ</c>, or <c>ZA</c>.
    /// - <c>company_reg</c>: a company registration number, such as a French SIREN. No <c>vat_country</c> is required.
    /// - <c>gln</c>: a Global Location Number. The value must be 13 digits.
    /// - <c>duns</c>: a D-U-N-S Number. The value must be 9 digits.
    /// - <c>lei</c>: a Legal Entity Identifier. The value must be 20 characters: 18 letters or digits followed by 2 digits.
    /// <para>
    /// A customer holds one identifier at a time. Saving an identifier of a different kind replaces the existing one.
    /// </para>
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("entity_identifier_kind")]
    public EntityIdentifierKind? EntityIdentifierKind { get; init; }

    /// <summary>
    /// The customer's tax or business identifier, sent together with <c>entity_identifier_kind</c>. Advanced Billing trims surrounding whitespace and stores the value in uppercase.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("entity_identifier_value")]
    public string? EntityIdentifierValue { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("tax_exempt")]
    public bool? TaxExempt { get; init; }

    /// <summary>
    /// Whether surcharging is enabled for the customer. Only applied on sites where surcharging control is enabled.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("surcharging")]
    public bool? Surcharging { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("tax_exempt_reason")]
    public string? TaxExemptReason { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("parent_id")]
    public int? ParentId { get; init; }

    /// <summary>
    /// Is the customer verified to use ACH as a payment method. Available only on the Authorize.Net gateway.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("verified")]
    public bool? Verified { get; init; }

    /// <summary>
    /// The Salesforce ID of the customer
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("salesforce_id")]
    public string? SalesforceId { get; init; }

    /// <summary>
    /// The ID of the Branding Theme assigned to this customer as the customer's default Branding Theme. This customer-level Branding Theme is used when a subscription does not have its own subscription-level Branding Theme. Available only when Branding Themes are enabled for the site.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("branding_theme_id")]
    public int? BrandingThemeId { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
