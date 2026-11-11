# 2021-11-23 Issue Service Credit Response Body Change

As of November 23rd, 2021, the [Issue Service Credit API endpoint]($e/Subscription%20Invoice%20Account/issueServiceCredit) will respond with a new body:

```json
{
  "id": "Integer",
  "amount_in_cents": "Integer",
  "ending_balance_in_cents": "Integer",
  "entry_type": "String",
  "memo": "String"
}
```

Previously, this endpoint was returning just a status code without body.
