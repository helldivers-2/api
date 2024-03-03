# Helldivers2
[![Fly Deploy](https://github.com/dealloc/helldivers2-api/actions/workflows/fly.yml/badge.svg)](https://github.com/dealloc/helldivers2-api/actions/workflows/fly.yml)
[![Docs](https://img.shields.io/badge/hex-docs-lightgreen.svg)](https://dealloc.github.io/helldivers2-api)
[![CI](https://github.com/dealloc/helldivers2-api/actions/workflows/elixir.yml/badge.svg)](https://github.com/dealloc/helldivers2-api/actions/workflows/elixir.yml)

This is a reverse engineered API of the official [Helldivers 2](https://store.steampowered.com/agecheck/app/553850/) video game.

This is **NOT** an official *nor* endorsed API and may stop functioning at any time.

> [!TIP]
> Hitting rate limits? You can run a local docker image, see [Local development](#running-in-docker--podman)

> [!TIP]
> Want to contribute even though you aren't a developer or don't know [Elixir](https://elixir-lang.org/)?
> You can help by finding more planet names, notifying us of new ongoing events
> or suggesting features!

### Getting started
Currently the API does not enforce authentication.
You can see all active war seasons at `/api/`

We provide an OpenApi specification which you can download at [`/api/openapi`](https://helldivers-2.fly.dev/api/openapi)

We also provide a SwaggerUI view on the above specification [here](https://helldivers-2.fly.dev/api/swaggerui).

### Rate limit
Currently the rate limit is set at 100 requests/5 minutes.
This limit will probably be increased in the future, but given the limited API endpoints available this should be sufficient.

To avoid hitting rate limits in your clients check the following headers in your response:
- `X-Ratelimit-Limit` contains the total amount of requests you can make in the given timeframe
- `X-RateLimit-Remaining` how many requests you can still make in the current window
- `X-RateLimit-Reset` the unix epoch (in milliseconds) when you can start making more requests
- `Retry-After` only added to 429 requests, the amount of seconds to wait before making a new request

### Roadmap
- [X] map /WarInfo & /Status
- [X] Provide endpoint for fetching WarInfo
- [X] Provide endpoint for fetching War status
- [X] Provide endpoint for fetching planet information
- [X] Provide endpoint for querying planets
- [ ] Provide endpoints for all mapped entities
- [ ] reverse-engineer entire Helldivers 2 API
- [X] tests
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

### Running in Docker / Podman
Want to run a private instance of the API, or simply want a local container without rate limits
so you can develop your app without being hit with rate limits constantly?

Fortunately, it's very easy to get a local/private copy of the Helldivers 2 API running!
All you need is to have either [Docker](https://www.docker.com/) or [Podman](https://podman.io/) installed.

You can build a local image like this (takes a few minutes, but you only need to re-run this when something changes):
```shell
# Docker
docker build -t helldivers2:latest .
# Or if you're using Podman
podman build -t helldivers2:latest .
```

Now to run this container, you'll need a few variables to configure how the API behaves (TLDR below!):
- `SECRET_KEY_BASE` used by the application for singing private or sensitive information, if you're running a private instance publicly you'll want a secure random string of 64 characters
- `FLY_APP_NAME` used internally for networking code to find other instances (you can just use `helldivers-2` for this)
- `FLY_IMAGE_REF` the container reference, this helps cluster to only the same version of the app (you can set this to `0`)
- `FLY_PRIVATE_IP` - used to uniquely identify this node in the cluster (if you're running a single instance you can use `127.0.0.1`)
- `RATE_LIMIT_MAX_REQUESTS` - (optional) the max amount of requests the rate limit will allow, set to `0` to disable rate limiting
- `RATE_LIMIT_INTERVAL` - (optional) how many seconds before the rate limit will reset itself (must be a valid number greater than `0`)

or if you're just setting up a local development instance, you can copy paste the command below:
```shell
# Docker
docker run -e SECRET_KEY_BASE=JlU7vU9xt9uWbf82Z9HrUjAuNGtqwqG8h8AaUc3AOyH0a86wa5Q4DITNLorGmILv -e FLY_APP_NAME=helldivers-2 -e FLY_IMAGE_REF=0 -e FLY_PRIVATE_IP=127.0.0.1 -p 4000:4000 helldivers2:latest
# Or if you're using Podman
podman run -e SECRET_KEY_BASE=JlU7vU9xt9uWbf82Z9HrUjAuNGtqwqG8h8AaUc3AOyH0a86wa5Q4DITNLorGmILv -e FLY_APP_NAME=helldivers-2 -e FLY_IMAGE_REF=0 -e FLY_PRIVATE_IP=127.0.0.1 -p 4000:4000 helldivers2:latest
```