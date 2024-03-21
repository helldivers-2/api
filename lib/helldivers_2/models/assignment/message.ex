defmodule Helldivers2.Models.Assignment.Message do
  alias Helldivers2.Models.Assignment.Setting
  alias Helldivers2.Models.Assignment.Reward

  @type t :: %__MODULE__{
    id32: non_neg_integer(),
    progress: list(non_neg_integer()),
    expiresIn: non_neg_integer(),
    setting: Setting.t()
  }

  defstruct [
    :id32,
    :progress,
    :expiresIn,
    :setting
  ]

  def parse(map) when is_map(map) do
    %__MODULE__{
      id32: Map.get(map, "id32"),
      progress: Map.get(map, "progress"),
      expiresIn: Map.get(map, "expiresIn"),
      setting: Map.get(map, "setting"),
    }
  end
end
