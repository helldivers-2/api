# Helldivers2

This is a reverse engineered API of the official [Helldivers 2](https://store.steampowered.com/agecheck/app/553850/) video game.

This is **NOT** an official *nor* endorsed API and may stop functioning at any time.

### Getting started
Currently the API does not enforce authentication.
You can see all active war seasons at `/api/`

We provide an OpenApi specification which you can download at [`/api/openapi`](https://helldivers-2.fly.dev/api/openapi)

We also provide a SwaggerUI view on the above specification [here](https://helldivers-2.fly.dev/api/swaggerui).

### Rate limit
Currently the rate limit is set rather low, at 10 requests/5 minutes.
This limit will probably be increased in the future, but given the limited API endpoints available this should be sufficient.

To avoid hitting rate limits in your clients check the following headers in your response:
- `X-Ratelimit-Limit` contains the total amount of requests you can make in the given timeframe
- `X-RateLimit-Remaining` how many requests you can still make in the current window
- `Retry-After` only added to 429 requests, the amount of seconds to wait before making a new request

### Roadmap
- [X] map /WarInfo & /Status
- [X] Provide endpoint for fetching WarInfo
- [ ] Provide endpoint for fetching War status
- [X] Provide endpoint for fetching planet information
- [ ] Provide endpoint for querying planets
- [ ] Provide endpoints for all mapped entities
- [ ] reverse-engineer entire Helldivers 2 API
- [ ] tests
- [ ] health checks

### Goals
It aims to achieve 2 things:
- Provide developers with an easy to use API into the Helldivers stats
- Cache the amount of API calls so that Helldivers' servers don't get overloaded *further*

### Local development
You'll need to have [Elixir](https://elixir-lang.org/install.html) installed, and I recommend following [Phoenix's](https://hexdocs.pm/phoenix/installation.html) installation guide.

Once you have cloned the local repository, set up everything with
```shell
mix setup
```

You can run an interactive shell and the application with
```shell
iex -S mix phx.server
```
