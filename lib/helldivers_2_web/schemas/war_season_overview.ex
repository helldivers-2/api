defmodule Helldivers2Web.Schemas.WarSeasonOverview do
  alias OpenApiSpex.Schema
  require OpenApiSpex

  def response(), do: {"Available war seasons", "application/json", __MODULE__, Helldivers2Web.ApiSpec.default_options()}

  OpenApiSpex.schema(%{
    description: "An overview of the available war seasons (and the current)",
    type: :object,
    properties: %{
      seasons: %Schema{type: :array, items: %Schema{type: :string}},
      current: %Schema{type: :string, description: "The currently active war season"}
    }
  })
end
