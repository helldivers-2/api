defmodule Helldivers2Web.Api.JointOperationsJSON do
  alias Helldivers2Web.Api.PlanetsJSON
  alias Helldivers2.Models.WarStatus.JointOperation
  def show(%JointOperation{} = joint_operation) do
    %{
      "id" => joint_operation.id,
      "planet" => PlanetsJSON.show(joint_operation.planet),
      "hq_node_index" => joint_operation.hq_node_index,
    }
  end
end
