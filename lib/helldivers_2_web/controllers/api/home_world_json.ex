defmodule Helldivers2Web.Api.HomeWorldJSON do
  alias Helldivers2Web.Api.PlanetsJSON
  alias Helldivers2.Models.WarInfo.HomeWorld

  def index(home_worlds) when is_list(home_worlds) do
    Enum.map(home_worlds, &show/1)
  end

  def show(%HomeWorld{} = home_world) do
    %{
      "race" => home_world.race,
      "planets" => PlanetsJSON.index(home_world.planets)
    }
  end
end
