defmodule Helldivers2.Models.WarStatus.JointOperation do
  alias Helldivers2.WarSeason
  alias Helldivers2.Models.WarInfo.Planet

  @type t :: %__MODULE__{
          id: non_neg_integer(),
          planet: Planet.t(),
          hq_node_index: non_neg_integer()
        }

  defstruct [
    :id,
    :planet,
    :hq_node_index
  ]

  @doc """
  Attempts to parse as much information as possible from the given `map` into a struct.
  """
  @spec parse(String.t(), map()) :: t()
  def parse(war_id, map) when is_map(map) do
    %__MODULE__{
      id: Map.get(map, "id"),
      planet: WarSeason.get_planet!(war_id, Map.get(map, "planetIndex")),
      hq_node_index: Map.get(map, "hqNodeIndex")
    }
  end
end
