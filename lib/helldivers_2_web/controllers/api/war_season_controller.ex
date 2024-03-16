defmodule Helldivers2Web.Api.WarSeasonController do
  use Helldivers2Web, :controller
  use OpenApiSpex.ControllerSpecs

  plug CastAndValidate, json_render_error_v2: true
  action_fallback Helldivers2Web.FallbackController

  alias Helldivers2.WarSeason
  alias Helldivers2Web.Schemas.WarInfoSchema
  alias Helldivers2Web.Schemas.WarStatusSchema
  alias Helldivers2Web.Schemas.WarSeasonOverview
  alias Helldivers2Web.Schemas.NewsFeedMessageSchema

  operation :index,
    summary: "Get an overview of all available war seasons",
    responses: [
      ok:
        {"Warseason overview", "application/json", WarSeasonOverview, Helldivers2Web.ApiSpec.default_options()},
      too_many_requests: TooManyRequestsSchema.response()
    ]

  def index(conn, _) do
    json(conn, %{
      current: Application.get_env(:helldivers_2, :war_season),
      seasons: Application.get_env(:helldivers_2, :war_seasons)
    })
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

  operation :show_status,
    summary: "Get the current status of the Helldivers offensive",
    externalDocs: %OpenApiSpex.ExternalDocumentation{
      description: "This is a mapped version of the official WarInfo object",
      url: "https://api.live.prod.thehelldiversgame.com/api/WarSeason/801/Status"
    },
    parameters: [
      war_id: [in: :path, description: "The war ID", type: :integer, example: 801]
    ],
    responses: [
      ok: WarStatusSchema.response(),
      not_found: NotFoundSchema.response(),
      too_many_requests: TooManyRequestsSchema.response(),
      unprocessable_entity: JsonErrorResponse.response()
    ]

  def show_status(conn, %{war_id: war_id}) do
    with {:ok, war_status} <- WarSeason.get_war_status(war_id) do
      render(conn, :show, war_status: war_status)
    end
  end

  operation :news_feed,
    summary: "Gets the newsfeed shown in-game under 'Dispatch'",
    externalDocs: %OpenApiSpex.ExternalDocumentation{
      description: "This is a mapped version of the official NewsFeed object",
      url: "https://api.live.prod.thehelldiversgame.com/api/NewsFeed/801"
    },
    parameters: [
      war_id: [in: :path, description: "The war ID", type: :integer, example: 801]
    ],
    responses: [
      ok: NewsFeedMessageSchema.responses(),
      not_found: NotFoundSchema.response(),
      too_many_requests: TooManyRequestsSchema.response(),
      unprocessable_entity: JsonErrorResponse.response()
    ]

  def news_feed(conn, %{war_id: war_id}) do
    with {:ok, news_feed} <- WarSeason.get_news_feed(war_id) do
      render(conn, :show, news_feed: news_feed)
    end
  end
end
