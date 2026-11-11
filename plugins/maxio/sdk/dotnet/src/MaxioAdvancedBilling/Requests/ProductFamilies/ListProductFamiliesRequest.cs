using System;
using MaxioAdvancedBilling.Models.Enums;

namespace MaxioAdvancedBilling.Requests.ProductFamilies;

/// <summary>
/// The inputs of the ListProductFamilies operation.
/// </summary>
public sealed record ListProductFamiliesRequest
{
    /// <summary>
    /// The type of filter you would like to apply to your search.
    /// Use in query: <c>date_field=created_at</c>.
    /// </summary>
    public BasicDateField? DateField { get; init; }

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
}
