# Helldivers2

This is a reverse engineered API of the official [Helldivers 2](https://store.steampowered.com/agecheck/app/553850/) video game.

This is **NOT** an official *nor* endorsed API and may stop functioning at any time.

### Getting started
Currently the API does not enforce authentication or rate limits (so be nice!)
You can see all active war seasons at `/api/`

Once you picked a war season you can query it with the currently implemented endpoints:
- `/api/:war_id/info`
- `/api/:war_id/planets`

for example: https://helldivers-2.fly.dev/api/801/info will yield the war season information of `801`

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
