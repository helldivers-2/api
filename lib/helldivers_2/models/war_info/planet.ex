defmodule Helldivers2.Models.WarInfo.Planet do
  alias Helldivers2.Models.WarInfo.Sector
  alias Helldivers2.Models.WarInfo.Faction

  use Helldivers2.Macros.FromJson, "planets.json"

  @type t() :: %__MODULE__{
          index: non_neg_integer(),
          name: String.t(),
          hash: String.t(),
          position: {float(), float()},
          waypoints: list(non_neg_integer()),
          sector: Sector.t(),
          max_health: non_neg_integer(),
          disabled: boolean(),
          initial_owner: Faction.t()
        }

  defstruct [
    :index,
    :name,
    :hash,
    :position,
    :waypoints,
    :sector,
    :max_health,
    :disabled,
    :initial_owner
  ]

  @doc """
  Attempts to parse as much information as possible from the given `map` into a struct.
  """
  @spec parse(map()) :: t()
  def parse(map) when is_map(map) do
    index = Map.get(map, "index")

    %__MODULE__{
      index: index,
      name: lookup(to_string(index)),
      hash: Map.get(map, "settingsHash"),
      position: parse_position(map),
      waypoints: parse_waypoints(map),
      sector: Sector.parse(Map.get(map, "sector")),
      max_health: Map.get(map, "maxHealth"),
      disabled: Map.get(map, "disabled"),
      initial_owner: Faction.parse(Map.get(map, "initialOwner"))
    }
  end

  defp parse_position(map) do
    position = Map.get(map, "position", %{})

    {round_coord(Map.get(position, "x") * 100), round_coord(Map.get(position, "y") * 100)}
  end

  defp parse_waypoints(map) do
    Map.get(map, "waypoints")
  end

  # define a fallback for FromJson.lookup
  defp lookup(_), do: "Unknown"

  defp round_coord(0), do: 0

  defp round_coord(number), do: Float.round(number, 6)
end
