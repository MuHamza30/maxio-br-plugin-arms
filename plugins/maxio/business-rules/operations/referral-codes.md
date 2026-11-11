# Referral Codes

## `client.ReferralCodes.ValidateReferralCode` — `GET /referral_codes/validate.json` (`validateReferralCode`)

- **loud**, other/errors: The referral code is not valid or not applicable within the site → 404 "Referral code is invalid.". **Do:** Treat 404 as an invalid code and do not pass it as ref when creating the subscription. *(spec `spec/openapi.yaml`; docs `content/introduction/about-the-api/expert-usage.md`; `maxio-c4:validateReferralCode.1`)*
