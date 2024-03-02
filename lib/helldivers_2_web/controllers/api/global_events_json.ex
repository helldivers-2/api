defmodule Helldivers2Web.Api.GlobalEventsJSON do
  alias Helldivers2Web.Api.PlanetsJSON
  alias Helldivers2.Models.WarStatus.GlobalEvent

  def index(%{global_events: global_events}), do: index(global_events)

  def index(global_events), do: Enum.map(global_events, &show/1)

  def show(%{global_event: global_event}), do: show(global_event)

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
      "effects" => global_event.effects,
      "planets" => Enum.map(global_event.planets, &PlanetsJSON.show/1)
    }
  end
end
