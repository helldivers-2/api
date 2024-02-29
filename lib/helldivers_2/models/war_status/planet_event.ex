defmodule Helldivers2.Models.WarStatus.PlanetEvent do
  alias Helldivers2.WarSeason
  alias Helldivers2.Models.WarStatus.JointOperation
  alias Helldivers2.Models.WarStatus.Campaign
  alias Helldivers2.Models.WarInfo.Faction
  alias Helldivers2.Models.WarInfo.Planet

  @type t :: %__MODULE__{
          id: non_neg_integer(),
          planet: Planet.t(),
          event_type: non_neg_integer(),
          race: Faction.t(),
          health: non_neg_integer(),
          max_health: non_neg_integer(),
          start_time: DateTime.t(),
          expire_time: DateTime.t(),
          campaign: Campaign.t(),
          joint_operations: list(JointOperation.t())
        }

  defstruct [
    :id,
    :planet,
    :event_type,
    :race,
    :health,
    :max_health,
    :start_time,
    :expire_time,
    :campaign,
    :joint_operations
  ]

  @doc """
  Attempts to parse as much information as possible from the given `map` into a struct.
  """
  @spec parse(String.t(), list(Campaign.t()), list(JointOperation.t()), map()) :: t()
  def parse(war_id, campaigns, joint_operations, map) when is_map(map) do
    %__MODULE__{
      id: Map.get(map, "id"),
      planet: WarSeason.get_planet!(war_id, Map.get(map, "planetIndex")),
      event_type: Map.get(map, "eventType"),
      race: Faction.parse(Map.get(map, "race")),
      health: Map.get(map, "health"),
      max_health: Map.get(map, "maxHealth"),
      start_time: DateTime.from_unix!(Map.get(map, "startTime")),
      expire_time: DateTime.from_unix!(Map.get(map, "expireTime")),
      campaign: lookup_campaign(map, campaigns),
      joint_operations:
        Map.get(map, "jointOperationIds")
        |> Enum.map(&lookup_joint_operation(joint_operations, &1))
        |> Enum.filter(fn operation -> !is_nil(operation) end)
    }
  end

  defp lookup_campaign(map, campaigns) do
    campaign_id = Map.get(map, "campaignId")

    Enum.find(campaigns, fn campaign ->
      campaign.id == campaign_id
    end)
  end

  defp lookup_joint_operation(joint_operations, joint_operation_id) do
    Enum.find(joint_operations, fn joint_operation ->
      joint_operation.id == joint_operation_id
    end)
  end
end
