defmodule Helldivers2Web.ApiSpec do
  alias OpenApiSpex.{Info, OpenApi, Paths, Server, Schema, Header}
  alias Helldivers2Web.{Endpoint, Router}
  @behaviour OpenApi

  @impl OpenApi
  def spec do
    %OpenApi{
      servers: [
        # Populate the Server info from a phoenix endpoint
        Server.from_endpoint(Endpoint)
      ],
      info: %Info{
        title: "Helldivers 2",
        version: "0.0.1"
      },
      # Populate the paths from a phoenix router
      paths: Paths.from_router(Router)
    }
    # Discover request/response schemas from path specs
    |> OpenApiSpex.resolve_schema_modules()
  end

  @doc "Generates the default options for responses, currently generates headers from `rate_limit_headers/1`."
  def default_options(options \\ []), do: options ++ [headers: rate_limit_headers()]

  @doc "Generates the headers of rate limit added by `Helldivers2Web.Plugs.RateLimit`."
  def rate_limit_headers(map \\ %{}) do
    Enum.into(map, %{
      "x-ratelimit-limit" => %Header{
        schema: %Schema{type: :number},
        description: "The total amount of requests that can be made in the time window"
      },
      "x-ratelimit-remaining" => %Header{
        schema: %Schema{type: :number},
        description: "The amount of requests remaining that can be made in the time window"
      },
      "x-ratelimit-reset" => %Header{
        schema: %Schema{type: :number},
        description: "The unix epoch timestamp (in seconds) when more requests can be made"
      }
    })
  end
end
