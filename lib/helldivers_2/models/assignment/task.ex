defmodule Helldivers2.Models.Assignment.Task do
  @type t :: %__MODULE__{
    type: non_neg_integer(),
    values: list(non_neg_integer()),
    valuesTypes: list(non_neg_integer()),
  }

  defstruct [
    :type,
    :values,
    :valuesTypes,
  ]

  def parse(map) when is_map(map) do
    %__MODULE__{
      type: Map.get(map, "type"),
      values: Map.get(map, "values"),
      valuesTypes: Map.get(map, "valuesTypes"),
    }
  end
end
