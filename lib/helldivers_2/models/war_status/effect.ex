defmodule Helldivers2.Models.WarStatus.Effect do
  @moduledoc """
  Describes a currently active effect, this is usually a buff or
  strategem currently being granted globally for all helldivers.
  """
  use Helldivers2.Macros.FromJson, "effects.json"

  @type t :: String.t()

  @spec parse(non_neg_integer() | String.t()) :: t()
  def parse(index) when is_number(index), do: parse(to_string(index))
  def parse(name), do: lookup(name)
  defp lookup(_), do: "Unknown"
end
