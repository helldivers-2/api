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
          players: non_neg_integer()
        }

  defstruct [
    :planet,
    :owner,
    :health,
    :regen_per_second,
    :players
  ]

  @doc """
  Attempts to parse as much information as possible from the given `map` into a struct.
  """
  @spec parse(String.t(), map()) :: t()
  def parse(war_id, map) when is_map(map) do
    %__MODULE__{
      planet: WarSeason.get_planet!(war_id, Map.get(map, "index")),
      owner: Faction.parse(Map.get(map, "owner")),
      health: Map.get(map, "health"),
      regen_per_second: Map.get(map, "regenPerSecond"),
      players: Map.get(map, "players")
    }
  end
end
