# Data Types

## Dates and Time Zones

Timestamp fields in API responses (e.g. `created_at`, `next_assessment_at`) are returned in the timezone of the Advanced Billing site, formatted as ISO 8601 with a UTC offset and no fractional seconds (for example, `2026-08-04T12:00:00-04:00`).

Some fields are dates only, with no time-of-day or timezone component (for example, `issue_date`, `due_date`, and `paid_date` on invoices and credit notes). These are plain `YYYY-MM-DD` strings, and the date itself reflects the site's local calendar date rather than UTC.

Webhook payloads use the timezone of the site the event belongs to (Eastern Time by default, if the site has not configured another timezone). Eastern Time observes daylight saving, so the UTC offset varies by time of year.

For requests:

- If only a date is provided, the API defaults to noon in the site's time zone.
- If a date and time are provided, the API uses that time of day in the site's time zone.
- If a date, time, and timezone offset are provided, the API uses that absolute instant and displays it in the site's time zone.

## Booleans, IDs, and Currency

Boolean fields are returned as native JSON `true`/`false` values.

Numeric `id` fields are JSON numbers. Some resources, such as invoices and credit notes, also expose a separate opaque string identifier, `uid`. This is distinct from the numeric `id` and should not be used interchangeably with it.

`currency` fields are strings using standard 3-letter ISO 4217 currency codes (e.g. `"USD"`).

## Decimal Numbers

In order to prevent losing precision, we serialize decimal numbers as strings instead of as JSON numbers.

We recommend parsing these strings into their decimal equivalent using a decimal number library in your programming language (for example, `BigDecimal` in Ruby) instead of relying on floating point values or arithmetic.

## Amount Fields and Currency

Fields holding amount values are given as a string representing a decimal currency amount.

For example, `"1.23"` in currency `"USD"` would equate to `$1.23`.

Not all amount fields are rounded to the currency's smallest denomination. Top-level totals (e.g., `total_amount`, `tax_amount`) are rounded to the smallest denomination of the currency. However, `unit_price` and `quantity` on line items can carry more decimal precision than the currency's smallest denomination, to support fractional and metered pricing.

It is up to API consumers to parse the string into a decimal number representation and do any rounding necessary for your application.
