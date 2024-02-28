defmodule Helldivers2Web.Schemas.NotFoundSchema do
  alias OpenApiSpex.Schema
  require OpenApiSpex

  def response(), do: {"Resource not found", "application/json", __MODULE__}

  OpenApiSpex.schema(%{
    description: "The resource you tried to retrieve could not be found",
    type: :object,
    properties: %{
      errors: %Schema{type: :object, properties: %{
        detail: %Schema{type: :string, description: "Description of what went wrong"}
      }}
    }
  })
end
