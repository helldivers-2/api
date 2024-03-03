defmodule Helldivers2.Models.WarStatus.PlanetStatus do
  @moduledoc """
  The current status of a planet in the galactic war.

  This contains the (current) owner, health, regen/second and
  currently active Helldivers on this planet.
  """
  alias Helldivers2.WarSeason
  alias Helldivers2.Models.WarInfo.Faction
  alias Helldivers2.Models.WarInfo.Planet

  @type t :: %__MODULE__{
          planet: Planet.t(),
          owner: Faction.t(),
          health: non_neg_integer(),
          regen_per_second: float(),
          players: non_neg_integer(),
          liberation: float(),
        }

  defstruct [
    :planet,
    :owner,
    :health,
    :regen_per_second,
    :players,
    :liberation
  ]

  @doc """
  Attempts to parse as much information as possible from the given `map` into a struct.
  """
  @spec parse(String.t(), map()) :: t()
  def parse(war_id, map) when is_map(map) do
    health = Map.get(map, "health")
    owner = Faction.parse(Map.get(map, "owner"))
    planet = WarSeason.get_planet!(war_id, Map.get(map, "index"))

    # Calculate health as a percentage
    health_percentage = ((health / planet.max_health) * 100)

    %__MODULE__{
      planet: planet,
      owner: owner,
      health: health,
      regen_per_second: Map.get(map, "regenPerSecond"),
      players: Map.get(map, "players"),
      liberation: liberation(owner, health_percentage)
    }
  end

  # Liberation depends on which faction controls, if it's Humans it's X% liberated.
  # If it's controlled by Automatons it's their control, so we deduct it from 100%
  defp liberation("Humans", percentage), do: percentage

  defp liberation(_faction, percentage), do: 100 - percentage
end
