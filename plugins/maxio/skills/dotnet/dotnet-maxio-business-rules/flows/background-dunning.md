# Background: dunning

- **silent**, -/webhooks (`background:dunning`, `background:webhook-delivery`): A subscription reaches a step of the dunning process → dunning_step_reached fires.. **Do:** Subscribe to dunning_step_reached to follow dunning progress. *(`maxio-c4:background.27`)*
- **silent**, -/side-effects (`background:dunning`): A credit card transaction fails or a card is expiring → Advanced Billing runs the dunning process (customer communication per the site's dunning plan) itself.. **Do:** Configure dunning plans in Advanced Billing rather than building your own retry emails. *(`maxio-c4:background.70`)*
