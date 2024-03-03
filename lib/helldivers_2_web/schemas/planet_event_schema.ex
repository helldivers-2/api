defmodule Helldivers2Web.Schemas.PlanetEventSchema do
  require OpenApiSpex

  alias Helldivers2Web.Schemas.JointOperationSchema
  alias Helldivers2Web.Schemas.CampaignSchema
  alias Helldivers2Web.Schemas.PlanetSchema
  alias OpenApiSpex.Schema

  @doc "Generates a schema for a single homeworld schema response"
  def response(), do: {"Planet event response", "application/json", __MODULE__, Helldivers2Web.ApiSpec.default_options()}

  OpenApiSpex.schema(%{
    description: "An event occuring on a specific planet for a limited time",
    type: :object,
    properties: %{
      id: %Schema{
        type: :integer,
        description: "The identifier of this event"
      },
      planet: PlanetSchema,
      event_type: %Schema{
        type: :integer,
        description: "Identifier of the type of event, haven't found an index for these yet"
      },
      race: %Schema{type: :string, description: "The race active in this event"},
      health: %Schema{
        type: :integer,
        description: "The current health of the planet in this event"
      },
      max_health: %Schema{
        type: :integer,
        description: "The max health pool of the planet in this event"
      },
      start_time: %Schema{
        type: :string,
        format: :"date-time",
        description: "When the event started on this planet"
      },
      expire_time: %Schema{
        type: :string,
        format: :"date-time",
        description: "When the event will end on this planet"
      },
      campaign: CampaignSchema,
      joint_operations: %Schema{type: :array, items: JointOperationSchema}
    }
  })
end
