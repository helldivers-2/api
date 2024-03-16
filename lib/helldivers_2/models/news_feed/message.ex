defmodule Helldivers2.Models.NewsFeed.Message do
  @type t :: %__MODULE__{
    id: non_neg_integer(),
    published_at: DateTime.t(),
    type: non_neg_integer(),
    tag_ids: list(non_neg_integer()),
    messages: String.t()
  }

  defstruct [
    :id,
    :published_at,
    :type,
    :tag_ids,
    :messages
  ]

  def parse(map, translations) when is_map(map) do
    # Get the ID of the base entity so we can fetch all matching entities from translations.
    id = Map.get(map, "id")
    published = Map.get(map, "published")
    messages = translations
    |> Map.new(fn {lang, events} ->
      message = events
      |> Enum.find(%{}, fn event -> Map.get(event, "id") == id end)
      |> Map.get("message")

      {lang, message}
    end)

    %__MODULE__{
      id: Map.get(map, "id"),
      published_at: DateTime.add(DateTime.utc_now(), -published),
      type: Map.get(map, "type"),
      tag_ids: Map.get(map, "tagIds"),
      messages: messages
    }
  end
end
