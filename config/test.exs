import Config

# We don't start up any Helldiver seasons during tests to avoid hitting the API
config :helldivers_2,
war_seasons: []

# We don't run a server during test. If one is required,
# you can enable the server option below.
config :helldivers_2, Helldivers2Web.Endpoint,
  http: [ip: {127, 0, 0, 1}, port: 4002],
  secret_key_base: "mwKZIC3HNHU53eNFSSNUfW9fL/nwvWa3JbHUTU6LbPFzG+hHQItYHvE8zisTj3EO",
  server: false

# Print only warnings and errors during test
config :logger, level: :warning

# Initialize plugs at runtime for faster test compilation
config :phoenix, :plug_init_mode, :runtime

# Configure rate limits, in tests we'll just have none
config :helldivers_2, Helldivers2Web.Plugs.RateLimit,
  max_requests: 0,
  # 5 minutes
  interval_seconds: 300
