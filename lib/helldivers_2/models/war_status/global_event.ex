defmodule Helldivers2.Models.WarStatus.GlobalEvent do
  alias Helldivers2.Models.WarStatus.Effect
  alias Helldivers2.WarSeason
  alias Helldivers2.Models.WarInfo.Planet
  alias Helldivers2.Models.WarInfo.Faction

  @type t :: %__MODULE__{
          id: non_neg_integer(),
          id_32: non_neg_integer(),
          portrait_id_32: non_neg_integer(),
          title: String.t(),
          title_32: non_neg_integer(),
          message: %{String.t() => String.t()},
          message_id_32: non_neg_integer(),
          race: Faction.t(),
          flag: non_neg_integer(),
          assignment_id_32: non_neg_integer(),
          effects: list(String.t()),
          planets: list(Planet.t())
        }

  defstruct [
    :id,
    :id_32,
    :portrait_id_32,
    :title,
    :title_32,
    :message,
    :message_id_32,
    :race,
    :flag,
    :assignment_id_32,
    :effects,
    :planets
  ]

  @doc """
  Attempts to parse as much information as possible from the given `map` into a struct.

  Takes an optional map of translations, where each key is the language
  and each value is the full list of all global events available.
  """
  @spec parse(String.t(), map()) :: t()
  def parse(war_id, map, translations \\ %{}) when is_map(map) do
    # Get the ID of the base entity so we can fetch all matching entities from translations.
    id = Map.get(map, "eventId")

    # Filter out our translations map to only include the currently being processed message.
    translations = translations
    |> Map.new(fn {lang, events} ->
      event = events
      |> Enum.find(%{}, fn event -> Map.get(event, "eventId") == id end)
      |> Map.get("message")

      {lang, event}
    end)

    %__MODULE__{
      id: id,
      id_32: Map.get(map, "id32"),
      portrait_id_32: Map.get(map, "portraitId32"),
      title: Map.get(map, "title"),
      title_32: Map.get(map, "titleId32"),
      message: translations,
      message_id_32: Map.get(map, "messageId32"),
      race: Faction.parse(Map.get(map, "race")),
      flag: Map.get(map, "flag"),
      assignment_id_32: Map.get(map, "assignmentId32"),
      effects: Enum.map(Map.get(map, "effectIds"), &Effect.parse/1),
      planets: Enum.map(Map.get(map, "planetIndices"), &WarSeason.get_planet!(war_id, &1))
    }
  end
end
