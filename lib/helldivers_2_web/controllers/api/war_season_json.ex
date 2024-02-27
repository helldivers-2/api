defmodule Helldivers2Web.Api.WarSeasonJSON do
  alias Helldivers2Web.Api.HomeWorldJSON
  alias Helldivers2Web.Api.PlanetsJSON
  alias Helldivers2.Models.WarInfo

  def show(%{war_info: war_info}), do: show(war_info)

  def show(%WarInfo{} = war_info) do
    %{
      "war_id" => war_info.war_id,
      "start_date" => war_info.start_date,
      "end_date" => war_info.end_date,
      "minimum_client_version" => war_info.minimum_client_version,
      "planets" => PlanetsJSON.index(war_info.planets),
      "home_worlds" => HomeWorldJSON.index(war_info.home_worlds),
      "capitals" => war_info.capitals,
      "planet_permanent_effects" => war_info.planet_permanent_effects
    }
  end
end
