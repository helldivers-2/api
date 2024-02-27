defmodule Helldivers2.Models.WarStatus.Campaign do
  alias Helldivers2.WarSeason
  alias Helldivers2.Models.WarInfo.Planet

  @type t :: %__MODULE__{
    id: number(),
    planet: Planet.t(),
    type: non_neg_integer(),
    count: non_neg_integer(),
  }

  defstruct [
    :id,
    :planet,
    :type,
    :count,
  ]

  @doc """
  Attempts to parse as much information as possible from the given `map` into a struct.
  """
  @spec parse(String.t(), map()) :: t()
  def parse(war_id, map) when is_map(map) do
    %__MODULE__{
      id: Map.get(map, "id"),
      planet: WarSeason.get_planet!(war_id, Map.get(map, "planetIndex")),
      type: Map.get(map, "type"),
      count: Map.get(map, "count")
    }
  end
end
