# Background: wallet-grant-expiration

- **silent**, state/side-effects (`background:wallet-grant-expiration`): A wallet grant reaches its expires_at before being fully spent → The remainder is written off automatically as a kind expiration ledger row, and the grant's expired_at/expired_on are filled.. **Do:** Expect expiration rows and a lower balance when grants expire. *(`maxio-c4:background.86`)*
