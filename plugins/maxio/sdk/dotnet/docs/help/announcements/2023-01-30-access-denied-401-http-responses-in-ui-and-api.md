# 2023-01-30 "Access Denied" 401 HTTP responses in UI and API

Between 12:54 UTC and 13:21 UTC, a routine system update being rolled out incrementally inadvertently caused a small number of customers to experience
"Access Denied" 401 HTTP status code responses in the UI and API.
As soon as our engineers determined the 401 responses were associated with the system upgrade, the upgrade was immediately rolled back.

Due to the incremental rollout of this system upgrade, only a small number of customers were affected within the 25-minute window between the initial rollout and subsequent reversion.
Customers were able to work around this issue by refreshing the page or retrying API calls until a successful response was received.

While this issue was corrected by the reversion of the system update, we sincerely apologize for any inconveniences this matter caused,
and thank you for your patience and understanding as our engineers worked toward a resolution.
