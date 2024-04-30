# helldivers-2/api

The `helldivers-2/api` project is a community API around the popular video game
[Helldivers 2](https://store.steampowered.com/app/553850/HELLDIVERS_2/?curator_clanid=33602140) by ArrowHead studios.
It provides (most) information you can also find in-game in a neat JSON format so you can build awesome applications
(see [Community](#community) below).

Important to note is that this is **not** an officially endorsed API and is in **no** way affiliated with ArrowHead studios,
but rather a community driven effort to provide the information of Helldivers 2 to it's community.

### Getting started
The API does not require authentication (unless you'd like higher rate limits than the default, see [rate limits](#rate-limits)),
so all you need to do is call it's publicly available endpoints.

We provide an [OpenAPI](https://helldivers-2.github.io/api/docs/openapi/Helldivers-2-API.json) specification of the community API as well as
a [SwaggerUI](https://helldivers-2.github.io/api/docs/openapi/swagger-ui.html) (which visualizes the OpenAPI document). We also provide an
[OpenAPI](https://helldivers-2.github.io/api/docs/openapi/Helldivers-2-API_arrowhead.json) of the official ArrowHead studio's API we use
internally, however we strongly encourage you to use the `/raw` endpoints of the community wrapper instead of accessing
the ArrowHead API directly, as it puts additional load on their servers (besides, we have shinier endpoints, I swear!).

The root URL of the API is available here: https://api.helldivers2.dev
> [!WARNING]
> The root domain of the API recently changed, it's recommended you use the domain above to avoid problems in the future

We ask that you send along a `X-Super-Client` header with the name of your application / domain
(e.g. `X-Super-Client: api.helldivers2.dev`) and optionally a `X-Super-Contact` with some form of contact if your site
does not link to any form of contact information we can find. We use this information in case we need to notify our users
of important changes to the API that may cause disruption of service or when additional restrictions would be imposed
on your app (to prevent abuse, unintentional or otherwise).

> [!IMPORTANT]
> While adding `X-Super-Client` and `X-Super-Contact` is currently not required, the `X-Super-Client` header **will**
> be made obligatory in the future, causing clients who don't send it to fail. For more information see #94

### Rate limits
Currently the rate limit is set at 5 requests/10 seconds.
This limit will probably be increased in the future, and is used during the transition to our new version.

To avoid hitting rate limits in your clients check the following headers in your response:
- `X-Ratelimit-Limit` contains the total amount of requests you can make in the given timeframe
- `X-RateLimit-Remaining` how many requests you can still make in the current window
- `Retry-After` only added to 429 requests, the amount of seconds to wait before making a new request

### Contributing
make sure to check out our [contribution](CONTRIBUTING.md) guidelines for more detailed information on how to
help us contributing!

### Community
Check out some awesome projects made by our community!
- [sebsebmc/helldivers-history](https://github.com/sebsebmc/helldivers-history)
  Dashboard and graphs made by git scraping the community made API 
- [stonemercy/galactic-wide-web](https://github.com/Stonemercy/Galactic-Wide-Web)
  The Galactic Wide Web - a discord bot
- [helldivers-2/api-wrapper](https://github.com/helldivers-2/api-wrapper)
  Typescript client code generated from OpenAPI
