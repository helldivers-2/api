defmodule Helldivers2.Models.WarInfo.Faction do
  @moduledoc """
  Wrapper module for fetching the faction from an index.

  See priv/factions.json for list
  """
  use Helldivers2.Macros.FromJson, "factions.json"

  @type t :: String.t()

  @spec parse(non_neg_integer() | String.t()) :: t()
  def parse(index) when is_number(index), do: parse(to_string(index))
  def parse(name), do: lookup(name)
  defp lookup(_), do: "Unknown"
end
