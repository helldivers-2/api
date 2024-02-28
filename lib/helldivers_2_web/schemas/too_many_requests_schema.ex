defmodule Helldivers2Web.Schemas.TooManyRequestsSchema do
  alias OpenApiSpex.Schema
  require OpenApiSpex

  def response(), do: {"Rate limit exceeded", "application/json", __MODULE__}

  OpenApiSpex.schema(%{
    description: "You're making too many requests in a limited span of time",
    type: :object,
    properties: %{
      error: %Schema{type: :string, description: "Error message for rate limit being exceeded"}
    }
  })
end
