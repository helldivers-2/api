defmodule Helldivers2Web.Schemas.CampaignSchema do
  alias Helldivers2Web.Schemas.PlanetSchema
  alias OpenApiSpex.Schema
  require OpenApiSpex

  @doc "Generates a schema for a single homeworld schema response"
  def response(), do: {"Campaign response", "application/json", __MODULE__, Helldivers2Web.ApiSpec.default_options()}

  OpenApiSpex.schema(%{
    description: "Contains information about a currently active campaign",
    type: :object,
    properties: %{
      id: %Schema{
        type: :integer,
        description: "The identifier of this campaign"
      },
      planet: PlanetSchema,
      type: %Schema{
        type: :integer,
        description: "The type of this campaign, haven't found out what they mean yet"
      },
      count: %Schema{
        type: :integer,
        description: "not sure what this counts, it's generally a low number"
      }
    }
  })
end
