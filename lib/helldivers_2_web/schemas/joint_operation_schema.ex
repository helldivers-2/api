defmodule Helldivers2Web.Schemas.JointOperationSchema do
  alias Helldivers2Web.Schemas.PlanetSchema
  alias OpenApiSpex.Schema
  require OpenApiSpex

  @doc "Generates a schema for a single homeworld schema response"
  def response(), do: {"Joint operation response", "application/json", __MODULE__, Helldivers2Web.ApiSpec.default_options()}

  OpenApiSpex.schema(%{
    description: "Contains information about a currently joint operation",
    type: :object,
    properties: %{
      id: %Schema{
        type: :integer,
        description: "The identifier of this campaign"
      },
      planet: PlanetSchema,
      hq_node_index: %Schema{
        type: :integer,
        description: "Haven't figured out what exactly this means"
      }
    }
  })
end
