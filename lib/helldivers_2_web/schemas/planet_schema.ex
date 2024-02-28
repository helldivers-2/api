defmodule Helldivers2Web.Schemas.PlanetSchema do
  alias OpenApiSpex.Schema
  require OpenApiSpex

  @doc "Generates a schema for a single planet schema response"
  def response(), do: {"Planet response", "application/json", __MODULE__}

  @doc "Generates a schema for an array of planet schemas"
  def responses(), do: {"Planets response", "application/json", %Schema{type: :array, items: __MODULE__}}

  OpenApiSpex.schema(%{
    description: "Represents a planet in the galactic war that must receive Managed democracy",
    type: :object,
    properties: %{
      index: %Schema{
        type: :integer,
        description:
          "The index of this planet, for convenience kept the same as in the official API"
      },
      name: %Schema{
        type: :string,
        description: "The human readable name of the planet, or unknown if it's not a known name"
      },
      hash: %Schema{
        type: :integer,
        description: "A hash retrieved from the official API, purpose unknown"
      },
      position: %Schema{
        type: :object,
        description: "The coordinates in the galaxy where this planet is located",
        properties: %{
          x: %Schema{type: :number, description: "X coordinate"},
          y: %Schema{type: :number, description: "Y coordinate"}
        }
      },
      waypoints: %Schema{
        type: :array,
        description: "Waypoints, seems to link planets together but purpose unclear"
      },
      max_health: %Schema{
        type: :integer,
        description: "The maximum health of this planet, used in conflict stats"
      },
      disabled: %Schema{
        type: :boolean,
        description: "Whether or not this planet is currently playable (enabled)"
      },
      initial_owner: %Schema{
        type: :string,
        description: "Which faction originally claimed this planet"
      }
    }
  })
end
