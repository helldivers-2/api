defmodule Helldivers2Web.Schemas.GlobalEventSchema do
  alias Helldivers2Web.Schemas.PlanetSchema
  alias OpenApiSpex.Schema
  require OpenApiSpex

  @doc "Generates a schema for a single homeworld schema response"
  def response(), do: {"Global event response", "application/json", __MODULE__}

  def responses(), do: {"Global events response", "application/json", %Schema{type: :array, items: __MODULE__}}

  OpenApiSpex.schema(%{
    description: "Contains information about a global event, past and present",
    type: :object,
    properties: %{
      id: %Schema{
        type: :integer,
        description: "The identifier of this campaign"
      },
      id_32: %Schema{type: :integer, description: "Internal identifier of this campaign, stable"},
      portrait_id_32: %Schema{
        type: :integer,
        description: "I suspect identifier of an in game image"
      },
      title: %Schema{
        type: :string,
        description: "The title text, for some reason this may not always be English"
      },
      title_32: %Schema{
        type: :integer,
        description:
          "Internal identifier of the title, this always remains the same regardless of language"
      },
      race: %Schema{
        type: :string,
        description: "The race involved in this campaign (so far seems to always be 'Human')"
      },
      flag: %Schema{
        type: :integer,
        description: "The identifier of the flag for this campaign (flags haven't been mapped)"
      },
      assignment_id_32: %Schema{
        type: :integer,
        description: "Internal identifier, haven't figured this out"
      },
      effect_ids: %Schema{
        type: :array,
        description: "A list of identifier, so far always been empty so no idea"
      },
      planets: %Schema{type: :array, items: PlanetSchema}
    }
  })
end
