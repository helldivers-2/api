defmodule Helldivers2Web.Schemas.PlanetStatusSchema do
  require OpenApiSpex

  alias OpenApiSpex.Schema
  alias Helldivers2Web.Schemas.PlanetSchema

  @doc "Generates a schema for a single war info schema response"
  def response(), do: {"Planet status response", "application/json", __MODULE__}

  OpenApiSpex.schema(%{
    description: "The current offense status of a planet (owner, health, regen, player count)",
    type: :object,
    properties: %{
      planet: PlanetSchema,
      owner: %Schema{
        type: :string,
        description: "The faction that owns the planet at this moment"
      },
      health: %Schema{type: :number, description: "The current 'health' of this planet"},
      regen_per_second: %Schema{
        type: :number,
        description: "At which rate this planet will regenerate it's health"
      },
      players: %Schema{
        type: :integer,
        description: "The amount of helldivers currently on this planet"
      },
      liberation: %Schema{
        type: :number,
        description: "The progression of liberation on this planet, presented as a %"
      }
    }
  })
end
