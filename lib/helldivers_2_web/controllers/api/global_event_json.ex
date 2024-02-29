defmodule Helldivers2Web.Api.GlobalEventJSON do
  alias Helldivers2Web.Api.PlanetsJSON
  alias Helldivers2.Models.WarStatus.GlobalEvent
  def show(%GlobalEvent{} = global_event) do
    %{
      "id" => global_event.id,
      "id_32" => global_event.id_32,
      "portrait_id_32" => global_event.portrait_id_32,
      "title" => global_event.title,
      "title_32" => global_event.title_32,
      "message" => global_event.message,
      "message_id_32" => global_event.message_id_32,
      "race" => global_event.race,
      "flag" => global_event.flag,
      "assignment_id_32" => global_event.assignment_id_32,
      "effect_ids" => global_event.effect_ids,
      "planets" => Enum.map(global_event.planets, &PlanetsJSON.show/1)
    }
  end
end
