defmodule Helldivers2Web.Schemas.WarInfoSchema do
  require OpenApiSpex

  alias OpenApiSpex.Schema
  alias Helldivers2Web.Schemas.{PlanetSchema, HomeWorldSchema}

  @doc "Generates a schema for a single war info schema response"
  def response(), do: {"War info response", "application/json", __MODULE__, Helldivers2Web.ApiSpec.default_options()}

  OpenApiSpex.schema(%{
    description: "Global overview of the war, it's planets, capitals etc",
    type: :object,
    properties: %{
      war_id: %Schema{
        type: :integer,
        description:
          "The identifier for this war, this ID must be passed for all resources under this war"
      },
      start_date: %Schema{
        type: :string,
        format: :"date-time",
        description: "When this war season was started"
      },
      end_date: %Schema{
        type: :string,
        format: :"date-time",
        description: "When this war season is scheduled to end"
      },
      minimum_client_version: %Schema{
        type: :string,
        description: "Used by the Helldivers 2 game client"
      },
      planets: %Schema{
        type: :array,
        items: PlanetSchema,
        description: "All planets present in this war season"
      },
      home_worlds: %Schema{
        type: :array,
        items: HomeWorldSchema,
        description: "All homeworlds present in this war season"
      },
      capitals: %Schema{type: :array, description: "Empty, not been mapped yet"},
      planet_permanent_effects: %Schema{type: :array, description: "Empty, not been mapped yet"}
    }
  })
end
