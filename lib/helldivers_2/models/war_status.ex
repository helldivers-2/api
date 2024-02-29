defmodule Helldivers2.Models.WarStatus do
  @moduledoc """
  Contains current information on the state of the galactic war.
  Specifically, the progression of democracy per planet,
  where attacks are taking place, ongoing campaigns,
  community targets, joint operations, planet events,
  and global events.
  """
  alias Helldivers2.WarSeason
  alias Helldivers2.Models.WarStatus.PlanetEvent
  alias Helldivers2.Models.WarStatus.JointOperation
  alias Helldivers2.Models.WarStatus.GlobalEvent
  alias Helldivers2.Models.WarStatus.Planet
  alias Helldivers2.Models.WarStatus.Campaign
  alias Helldivers2.Models.WarStatus.PlanetStatus

  @type t :: %__MODULE__{
          war_id: String.t(),
          snapshot_at: DateTime.t(),
          impact_multiplier: float(),
          planet_status: list(PlanetStatus.t()),
          planet_attacks: {Planet.t(), Planet.t()},
          campaigns: list(Campaign.t()),
          community_targets: [],
          joint_operations: list(JointOperation.t()),
          planet_events: list(PlanetEvent.t()),
          planet_active_effects: [],
          active_election_policy_effects: [],
          global_events: list(GlobalEvent.t())
        }

  defstruct [
    :war_id,
    :snapshot_at,
    :impact_multiplier,
    :planet_status,
    :planet_attacks,
    :campaigns,
    :community_targets,
    :joint_operations,
    :planet_events,
    :planet_active_effects,
    :active_election_policy_effects,
    :global_events
  ]

  @spec download(String.t()) :: {:ok, t()} | {:error, term()}
  def download(war_id) do
    with {:ok, response} <-
           Req.get("https://api.live.prod.thehelldiversgame.com/api/WarSeason/#{war_id}/Status"),
         %Req.Response{status: 200, body: payload} <- response do
      {:ok, parse(payload)}
    else
      %Req.Response{status: status} ->
        {:error, "API error #{status}"}
    end
  end

  @doc """
  Attempts to parse as much information as possible from the given `map` into a struct.
  """
  @spec parse(map()) :: t()
  def parse(map) when is_map(map) do
    war_id = Map.get(map, "warId")
    campaigns = Enum.map(Map.get(map, "campaigns"), &Campaign.parse(war_id, &1))

    joint_operations =
      Enum.map(Map.get(map, "jointOperations"), &JointOperation.parse(war_id, &1))

    %__MODULE__{
      war_id: war_id,
      snapshot_at: DateTime.from_unix!(Map.get(map, "time")),
      impact_multiplier: Map.get(map, "impactMultiplier"),
      planet_status: Enum.map(Map.get(map, "planetStatus"), &PlanetStatus.parse(war_id, &1)),
      planet_attacks:
        Enum.map(Map.get(map, "planetAttacks"), fn attack ->
          {
            WarSeason.get_planet!(war_id, Map.get(attack, "source")),
            WarSeason.get_planet!(war_id, Map.get(attack, "target"))
          }
        end),
      campaigns: campaigns,
      community_targets: [],
      joint_operations: joint_operations,
      planet_events:
        Enum.map(
          Map.get(map, "planetEvents"),
          &PlanetEvent.parse(war_id, campaigns, joint_operations, &1)
        ),
      planet_active_effects: [],
      active_election_policy_effects: [],
      global_events: Enum.map(Map.get(map, "globalEvents"), &GlobalEvent.parse(war_id, &1))
    }
  end
end
