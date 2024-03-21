defmodule Helldivers2.Models.Assignments.Assignment.Reward do
  @type t :: %__MODULE__{
    amount: non_neg_integer(),
    id32: non_neg_integer(),
    type: non_neg_integer(),
  }

  defstruct [
    :amount,
    :id32,
    :type,
  ]

  def parse(map) when is_map(map) do
    %__MODULE__{
      amount: Map.get(map, "amount"),
      id32: Map.get(map, "id32"),
      type: Map.get(map, "type"),
    }
  end
end
