defmodule Helldivers2.Models.Assignments.Assignment.Setting do
  alias Helldivers2.Models.Assignments.Assignment.Reward
  alias Helldivers2.Models.Assignments.Assignment.Task

  @type t :: %__MODULE__{
    type: non_neg_integer(),
    overrideTitle: String.t(),
    overrideBrief: String.t(),
    taskDescription: String.t(),
    tasks: list(Task.t()),
    reward: Reward.t(),
    flags: non_neg_integer(),
  }

  defstruct [
    :type,
    :overrideTitle,
    :overrideBrief,
    :taskDescription,
    :tasks,
    :reward,
    :flags,
  ]

  def parse(map) when is_map(map) do
    %__MODULE__{
      type: Map.get(map, "type"),
      overrideTitle: Map.get(map, "overrideTitle"),
      overrideBrief: Map.get(map, "overrideBrief"),
      taskDescription: Map.get(map, "taskDescription"),
      tasks: Map.get(map, "tasks"),
      reward: Map.get(map, "reward"),
      flags: Map.get(map, "flags"),
    }
  end
end
