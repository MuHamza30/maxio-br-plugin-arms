using System;
using MaxioAdvancedBilling.Core.Validation.Attributes;
using MaxioAdvancedBilling.Models;
using MaxioAdvancedBilling.Models.Enums;

namespace MaxioAdvancedBilling.Requests.ProductFamilies;

/// <summary>
/// The inputs of the ListProductsForProductFamily operation.
/// </summary>
public sealed record ListProductsForProductFamilyRequest
{
    /// <summary>
    /// Either the product family's id or its handle prefixed with <c>handle:</c>
    /// </summary>
    public required string ProductFamilyId { get; init; }

    /// <summary>
    /// Result records are organized in pages. By default, the first page of results is displayed. The page parameter specifies a page number of results to fetch. You can start navigating through the pages to consume the results. You do this by passing in a page parameter. Retrieve the next page by adding ?page=2 to the query string. If there are no results to return, then an empty result set will be returned.
    /// Use in query <c>page=1</c>.
    /// </summary>
    [Minimum(1)]
    public int Page { get; init; } = 1;

    /// <summary>
    /// This parameter indicates how many records to fetch in each request. Default value is 20. The maximum allowed values is 200; any per_page value over 200 will be changed to 200.
    /// Use in query <c>per_page=200</c>.
    /// </summary>
    [Maximum(200)]
    public int PerPage { get; init; } = 20;

    /// <summary>
    /// The type of filter you would like to apply to your search.
    /// Use in query: <c>date_field=created_at</c>.
    /// </summary>
    public BasicDateField? DateField { get; init; }

    /// <summary>
    /// Filter to use for List Products operations
    /// </summary>
    public ListProductsFilter? Filter { get; init; }

    /// <summary>
    /// The start date (format YYYY-MM-DD) with which to filter the date_field. Returns products with a timestamp at or after midnight (12:00:00 AM) in your site’s time zone on the date specified.
    /// </summary>
    public DateTimeOffset? StartDate { get; init; }

    /// <summary>
    /// The end date (format YYYY-MM-DD) with which to filter the date_field. Returns products with a timestamp up to and including 11:59:59PM in your site’s time zone on the date specified.
    /// </summary>
    public DateTimeOffset? EndDate { get; init; }

    /// <summary>
    /// The start date and time (format YYYY-MM-DD HH:MM:SS) with which to filter the date_field. Returns products with a timestamp at or after exact time provided in query. You can specify timezone in query - otherwise your site's time zone will be used. If provided, this parameter will be used instead of start_date.
    /// </summary>
    public DateTimeOffset? StartDatetime { get; init; }

    /// <summary>
    /// The end date and time (format YYYY-MM-DD HH:MM:SS) with which to filter the date_field. Returns products with a timestamp at or before exact time provided in query. You can specify timezone in query - otherwise your site's time zone will be used. If provided, this parameter will be used instead of end_date.
    /// </summary>
    public DateTimeOffset? EndDatetime { get; init; }

    /// <summary>
    /// Include archived products.
    /// </summary>
    public bool? IncludeArchived { get; init; }

    /// <summary>
    /// Allows including additional data in the response. Use in query <c>include=prepaid_product_price_point</c>.
    /// </summary>
    public ListProductsInclude? Include { get; init; }
}
