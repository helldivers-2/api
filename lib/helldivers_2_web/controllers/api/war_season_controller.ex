defmodule Helldivers2Web.Api.WarSeasonController do
  use Helldivers2Web, :controller
  use OpenApiSpex.ControllerSpecs

  plug CastAndValidate, json_render_error_v2: true
  action_fallback Helldivers2Web.FallbackController

  alias Helldivers2.WarSeason
  alias Helldivers2Web.Schemas.WarInfoSchema

  operation :index,
    summary: "Get an overview of all available war seasons",
    responses: [
      ok:
        {"Warseason overview", "application/json",
         %Schema{type: :array, items: %Schema{type: :string}}},
      too_many_requests: TooManyRequestsSchema.response()
    ]

  def index(conn, _) do
    json(conn, Application.get_env(:helldivers_2, :war_seasons))
  end

  operation :show_info,
    summary: "Get information on a war season",
    externalDocs: %OpenApiSpex.ExternalDocumentation{
      description: "This is a mapped version of the official WarInfo object",
      url: "https://api.live.prod.thehelldiversgame.com/api/WarSeason/801/WarInfo"
    },
    parameters: [
      war_id: [in: :path, description: "The war ID", type: :integer, example: 801]
    ],
    responses: [
      ok: WarInfoSchema.response(),
      not_found: NotFoundSchema.response(),
      too_many_requests: TooManyRequestsSchema.response(),
      unprocessable_entity: JsonErrorResponse.response()
    ]

  def show_info(conn, %{war_id: war_id}) do
    with {:ok, war_info} <- WarSeason.get_war_info(war_id) do
      render(conn, :show, war_info: war_info)
    end
  end
end
