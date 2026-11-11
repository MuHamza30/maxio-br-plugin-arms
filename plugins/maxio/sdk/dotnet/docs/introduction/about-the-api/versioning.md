# Versioning

## Backwards Compatibility

We consider the following changes to be backwards compatible and may make them without advance notice:

- Adding new API endpoints, or adding new attributes in the responses of existing endpoints
- Adding new optional parameters to be sent to existing API endpoints
- Adding new fields to exported data
- Changing the type or length of any of the ID attributes
  - For example, most IDs are currently integers, but you should not assume that this will always be the case.

In addition, you should not depend on the order of attributes within the API response as this may change.

Advanced Billing does not provide notifications for additions that are clearly defined as backwards compatible.
