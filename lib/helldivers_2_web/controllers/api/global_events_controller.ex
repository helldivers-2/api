defmodule Helldivers2Web.Api.GlobalEventsController do
  alias Helldivers2.WarSeason
  use Helldivers2Web, :controller
  use OpenApiSpex.ControllerSpecs

  plug CastAndValidate, json_render_error_v2: true
  action_fallback Helldivers2Web.FallbackController

  alias Helldivers2Web.Schemas.GlobalEventSchema

  operation :index,
    summary: "Get an overview of all global events",
    parameters: [
      war_id: [in: :path, description: "The war ID", type: :integer, example: 801]
    ],
    responses: [
      ok: GlobalEventSchema.responses(),
      not_found: NotFoundSchema.response(),
      too_many_requests: TooManyRequestsSchema.response()
    ]

  def index(conn, %{war_id: war_id}) do
    with {:ok, war_status} <- WarSeason.get_war_status(war_id) do
      render(conn, :index, global_events: war_status.global_events)
    end
  end

  operation :latest,
    summary: "Get the latest global event",
    parameters: [
      war_id: [in: :path, description: "The war ID", type: :integer, example: 801]
    ],
    responses: [
      ok: GlobalEventSchema.response(),
      not_found: NotFoundSchema.response(),
      too_many_requests: TooManyRequestsSchema.response()
    ]
  def latest(conn, %{war_id: war_id}) do
    with {:ok, war_status} <- WarSeason.get_war_status(war_id) do
      case Enum.take(war_status.global_events, 1) do
        [] ->
          {:error, :not_found}

        [event] ->
          render(conn, :show, global_event: event)
      end
    end
  end
end
