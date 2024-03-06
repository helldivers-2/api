defmodule Helldivers2Web.Schemas.WarStatusSchema do
  require OpenApiSpex

  alias Helldivers2Web.Schemas.GlobalEventSchema
  alias Helldivers2Web.Schemas.PlanetEventSchema
  alias Helldivers2Web.Schemas.JointOperationSchema
  alias Helldivers2Web.Schemas.CampaignSchema
  alias Helldivers2Web.Schemas.PlanetSchema
  alias Helldivers2Web.Schemas.PlanetStatusSchema
  alias OpenApiSpex.Schema

  @doc "Generates a schema for a single war info schema response"
  def response(),
    do:
      {"War status response", "application/json", __MODULE__,
       Helldivers2Web.ApiSpec.default_options()}

  OpenApiSpex.schema(%{
    description: "Current status of the Helldivers offensive in the galactic war",
    type: :object,
    properties: %{
      war_id: %Schema{
        type: :integer,
        description:
          "The identifier for this war, this ID must be passed for all resources under this war"
      },
      snapshot_at: %Schema{
        type: :string,
        format: :"date-time",
        description:
          "The timestamp this status was snapshotted, as returned by the Helldivers API"
      },
      impact_multiplier: %Schema{
        type: :number,
        description: "I don't fully understand what this does, feel free to ping me if you know"
      },
      planet_status: %Schema{type: :array, items: PlanetStatusSchema},
      planet_attacks: %Schema{
        type: :array,
        items: %Schema{
          type: :object,
          properties: %{
            source: PlanetSchema,
            target: PlanetSchema
          }
        },
        description: "An overview of attacks currently being carried out against Democracy"
      },
      campaigns: %Schema{
        type: :array,
        items: CampaignSchema,
        description: "An overview of the campaigns active in the current offensive"
      },
      community_targets: %Schema{
        type: :array,
        items: %Schema{type: :integer},
        description: "Always empty AFAIK, haven't figured this out"
      },
      joint_operations: %Schema{type: :array, items: JointOperationSchema},
      planet_events: %Schema{type: :array, items: PlanetEventSchema},
      planet_active_effects: %Schema{
        type: :array,
        items: %Schema{type: :integer},
        description: "Always empty AFAIK, haven't figured this out"
      },
      active_election_policy_effects: %Schema{
        type: :array,
        items: %Schema{type: :integer},
        description: "Always empty AFAIK, haven't figured this out"
      },
      global_events: %Schema{type: :array, items: GlobalEventSchema}
    }
  })
end
