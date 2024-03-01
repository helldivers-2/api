defmodule Helldivers2Web.Api.WarSeasonJSON do
  alias Helldivers2Web.Api.GlobalEventJSON
  alias Helldivers2Web.Api.JointOperationsJSON
  alias Helldivers2Web.Api.CampaignJSON
  alias Helldivers2.Models.WarStatus
  alias Helldivers2Web.Api.HomeWorldJSON
  alias Helldivers2Web.Api.PlanetsJSON
  alias Helldivers2.Models.WarInfo

  def show(%{war_info: war_info}), do: show(war_info)
  def show(%{war_status: war_status}), do: show(war_status)

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

  def show(%WarStatus{} = war_status) do
    %{
      "war_id" => war_status.war_id,
      "snapshot_at" => war_status.snapshot_at,
      "impact_multiplier" => war_status.impact_multiplier,
      "planet_status" => Enum.map(war_status.planet_status, &PlanetsJSON.show_status/1),
      "planet_attacks" =>
        Enum.map(war_status.planet_attacks, fn {source, target} ->
          %{
            "source" => PlanetsJSON.show(source),
            "target" => PlanetsJSON.show(target)
          }
        end),
      "campaigns" => Enum.map(war_status.campaigns, &CampaignJSON.show/1),
      "community_targets" => [],
      "joint_operations" => Enum.map(war_status.joint_operations, &JointOperationsJSON.show/1),
      "planet_events" => Enum.map(war_status.planet_events, &PlanetsJSON.show_event/1),
      "planet_active_effects" => [],
      "active_election_policy_effects" => [],
      "global_events" => Enum.map(war_status.global_events, &GlobalEventJSON.show/1)
    }
  end
end
