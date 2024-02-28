defmodule Helldivers2Web.ApiSpec do
  alias OpenApiSpex.{Info, OpenApi, Paths, Server}
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
    |> OpenApiSpex.resolve_schema_modules() # Discover request/response schemas from path specs
  end
end
