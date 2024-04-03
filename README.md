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

We provide an [OpenAPI](src/Helldivers-2-API/wwwroot/Helldivers-2-API.json) specification of the community API as well as
a [SwaggerUI](https://helldivers-2.fly.dev/swagger-ui.html) (which visualizes the OpenAPI document). We also provide an
[OpenAPI](src/Helldivers-2-API/wwwroot/Helldivers-2-API_arrowhead.json) of the official ArrowHead studio's API we use
internally, however we strongly encourage you to use the `/raw` endpoints of the community wrapper instead of accessing
the ArrowHead API directly, as it puts additional load on their servers (besides, we have shinier endpoints, I swear!).

The root URL of the API is available here: https://helldivers-2-dotnet.fly.dev/
> [!WARNING]
> Note that it might change as we are transitioning from the Elixir version to a new version!

We also ask that you send us a `User-Agent` header when making requests (if accessing directly from the browser,
the headers sent by those should suffice and you don't need to add anything special).
While this is currently not *required*, we are considering making this required in the future, so adding it now
is the safer option.

We also ask that you include an `X-Application-Contact` header with either a Discord, email or other contact handle
so we know how to reach out to you (see below).

We ask this so we can identify the applications making requests, and so we can reach out in case we notice weird or
incorrect behaviour (or we notice you're generating more traffic than we can handle).

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
