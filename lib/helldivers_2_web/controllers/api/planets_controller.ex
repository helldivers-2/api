defmodule Helldivers2Web.Api.PlanetsController do
  use Helldivers2Web, :controller

  action_fallback Helldivers2Web.FallbackController

  alias Helldivers2.WarSeason

  def index(conn, %{"war_id" => war_id}) do
    with {:ok, planets} <- WarSeason.get_planets(war_id) do
      render(conn, :index, planets: planets)
    end
  end

  def show(conn, %{"war_id" => war_id, "planet_index" => planet_index}) do
    with {planet_index, _} <- Integer.parse(planet_index),
         {:ok, planets} <- WarSeason.get_planet(war_id, planet_index) do
      render(conn, :show, planet: planets)
    end
  end
end
