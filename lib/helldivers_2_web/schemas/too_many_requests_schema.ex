defmodule Helldivers2Web.Schemas.TooManyRequestsSchema do
  alias Helldivers2Web.ApiSpec
  alias OpenApiSpex.Header
  alias OpenApiSpex.Schema
  require OpenApiSpex

  def response() do
    {"Rate limit exceeded", "application/json", __MODULE__,
     [
       headers:
         ApiSpec.rate_limit_headers(%{
           "retry-after" => %Header{
             schema: %Schema{type: :number},
             description: "The amount of seconds to wait before making new requests"
           }
         })
     ]}
  end

  OpenApiSpex.schema(%{
    description: "You're making too many requests in a limited span of time",
    type: :object,
    properties: %{
      error: %Schema{type: :string, description: "Error message for rate limit being exceeded"}
    }
  })
end
