defmodule Helldivers2Web.Api.PlanetsJSON do
  alias Helldivers2.Models.WarInfo.Planet
  def index(%{planets: planets}), do: index(planets)

  def index(planets) when is_list(planets) do
    Enum.map(planets, &show/1)
  end

  def show(%{planet: planet}), do: show(planet)
  def show(%Planet{} = planet) do
    {x, y} = planet.position

    %{
      "index" => planet.index,
      "name" => planet.name,
      "hash" => planet.hash,
      "position" => %{
        "x" => x,
        "y" => y
      },
      "waypoints" => planet.waypoints,
      "sector" => planet.sector,
      "max_health" => planet.max_health,
      "disabled" => planet.disabled,
      "initial_owner" => planet.initial_owner
    }
  end
end
