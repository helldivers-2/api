defmodule Helldivers2.Models.WarInfo.HomeWorld do
  @moduledoc """
  Represents information about the homeworld of a given faction.

  There's currently 3 factions in Helldivers 2:
  - Humans
  - Automatons
  - Terminid
  """
  alias Helldivers2.Models.WarInfo.Planet
  alias Helldivers2.Models.WarInfo.Faction

  @type t :: %__MODULE__{
    race: Faction.t(),
    planets: list(Planet.t()),
  }

  defstruct [
    :race,
    :planets,
  ]

  @doc """
  Attempts to parse as much information as possible from the given `map` into a struct.
  """
  @spec parse(map(), list(Planet.t())) :: t()
  def parse(map, planets) when is_map(map) do
    %__MODULE__{
      race: Faction.parse(Map.get(map, "race")),
      planets: Enum.map(Map.get(map, "planetIndices"), &(find_planet(&1, planets)))
    }
  end

  @spec find_planet(non_neg_integer(), list(Planet.t())) :: Planet.t()
  defp find_planet(planet_index, planets) do
    Enum.find(planets, fn (planet) ->
      planet.index == planet_index
    end)
  end
end
