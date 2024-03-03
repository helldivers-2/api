defmodule Helldivers2Web.Schemas.HomeWorldSchema do
  alias Helldivers2Web.Schemas.PlanetSchema
  alias OpenApiSpex.Schema
  require OpenApiSpex

  @doc "Generates a schema for a single homeworld schema response"
  def response(), do: {"Homeworld response", "application/json", __MODULE__, Helldivers2Web.ApiSpec.default_options()}

  OpenApiSpex.schema(%{
    description: "Homeworld information of a given faction",
    type: :object,
    properties: %{
      race: %Schema{
        type: :string,
        description: "The race that the planet is the homeworld of"
      },
      planets: %Schema{
        type: :array,
        items: PlanetSchema,
        description: "The planets this race considers it's homeworlds"
      }
    }
  })
end
