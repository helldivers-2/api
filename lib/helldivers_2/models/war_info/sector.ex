defmodule Helldivers2.Models.WarInfo.Sector do
  @moduledoc """
  Wrapper module for fetching the sector from an index.
  """
  use Helldivers2.Macros.FromJson, "sectors.json"

  @type t :: String.t()

  @spec parse(non_neg_integer() | String.t()) :: t()
  def parse(index) when is_number(index), do: parse(to_string(index))
  def parse(name), do: lookup(name)
  defp lookup(index), do: index
end
