defmodule Helldivers2Web.Api.PlanetsJSON do
  alias Helldivers2Web.Api.JointOperationsJSON
  alias Helldivers2Web.Api.CampaignJSON
  alias Helldivers2.Models.WarStatus.PlanetEvent
  alias Helldivers2.Models.WarStatus.PlanetStatus
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

  @doc "Named separately from `show/1` to avoid pattern matching conflicts (`PlanetStatus` also matches for `%{planet: planet}`)"
  def show_status(%{planet_status: planet_status}), do: show_status(planet_status)
  def show_status(%PlanetStatus{} = planet_status) do
    %{
      "planet" => show(planet_status.planet),
      "owner" => planet_status.owner,
      "health" => planet_status.health,
      "regen_per_second" => planet_status.regen_per_second,
      "players" => planet_status.players,
    }
  end

  def show_event(%PlanetEvent{} = planet_event) do
    %{
      "id" => planet_event.id,
      "planet" => show(planet_event.planet),
      "event_type" => planet_event.event_type,
      "race" => planet_event.race,
      "health" => planet_event.health,
      "max_health" => planet_event.max_health,
      "start_time" => planet_event.start_time,
      "expire_time" => planet_event.expire_time,
      "campaign" => CampaignJSON.show(planet_event.campaign),
      "joint_operations" => Enum.map(planet_event.joint_operations, &JointOperationsJSON.show/1)
    }
  end
end
