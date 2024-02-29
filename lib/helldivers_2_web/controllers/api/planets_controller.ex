defmodule Helldivers2Web.Api.PlanetsController do
  alias Helldivers2Web.Schemas.PlanetStatusSchema
  use Helldivers2Web, :controller
  use OpenApiSpex.ControllerSpecs

  plug CastAndValidate, json_render_error_v2: true
  action_fallback Helldivers2Web.FallbackController

  alias Helldivers2.WarSeason
  alias Helldivers2Web.Schemas.PlanetSchema

  operation :index,
    summary: "Get an overview of all planets",
    parameters: [
      war_id: [in: :path, description: "The war ID", type: :integer, example: 801]
    ],
    responses: [
      ok: PlanetSchema.responses(),
      not_found: NotFoundSchema.response(),
      too_many_requests: TooManyRequestsSchema.response()
    ]
  def index(conn, %{war_id: war_id}) do
    with {:ok, planets} <- WarSeason.get_planets(war_id) do
      render(conn, :index, planets: planets)
    end
  end

  operation :show,
    summary: "Get information on a specific planet",
    parameters: [
      war_id: [in: :path, description: "The war ID", type: :integer, example: 801],
      planet_index: [
        in: :path,
        description: "The index of the planet",
        type: :integer,
        example: 0
      ]
    ],
    responses: [
      ok: PlanetSchema.response(),
      not_found: NotFoundSchema.response(),
      too_many_requests: TooManyRequestsSchema.response(),
      unprocessable_entity: JsonErrorResponse.response()
    ]
  def show(conn, %{war_id: war_id, planet_index: planet_index}) do
    with {:ok, planets} <- WarSeason.get_planet(war_id, planet_index) do
      render(conn, :show, planet: planets)
    end
  end

  operation :show_planet_status,
    summary: "Get the current war status of a specific planet",
    parameters: [
      war_id: [in: :path, description: "The war ID", type: :integer, example: 801],
      planet_index: [
        in: :path,
        description: "The index of the planet",
        type: :integer,
        example: 0
      ]
    ],
    responses: [
      ok: PlanetStatusSchema.response(),
      not_found: NotFoundSchema.response(),
      too_many_requests: TooManyRequestsSchema.response(),
      unprocessable_entity: JsonErrorResponse.response()
    ]
  def show_planet_status(conn, %{war_id: war_id, planet_index: planet_index}) do
    with {:ok, planet_status} <- WarSeason.get_planet_status(war_id, planet_index) do
      render(conn, :show_status, planet_status: planet_status)
    end
  end
end
