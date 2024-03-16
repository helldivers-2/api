defmodule Helldivers2.Models.WarStatus do
  @moduledoc """
  Contains current information on the state of the galactic war.
  Specifically, the progression of democracy per planet,
  where attacks are taking place, ongoing campaigns,
  community targets, joint operations, planet events,
  and global events.
  """
  require Logger
  alias Helldivers2.WarSeason
  alias Helldivers2.Models.WarInfo.Planet
  alias Helldivers2.Models.WarStatus.PlanetEvent
  alias Helldivers2.Models.WarStatus.JointOperation
  alias Helldivers2.Models.WarStatus.GlobalEvent
  alias Helldivers2.Models.WarStatus.Campaign
  alias Helldivers2.Models.WarStatus.PlanetStatus

  @type t :: %__MODULE__{
          war_id: String.t(),
          started_at: DateTime.t(),
          snapshot_at: DateTime.t(),
          impact_multiplier: float(),
          planet_status: list(PlanetStatus.t()),
          planet_attacks: {Planet.t(), Planet.t()},
          campaigns: list(Campaign.t()),
          community_targets: list(),
          joint_operations: list(JointOperation.t()),
          planet_events: list(PlanetEvent.t()),
          planet_active_effects: list(),
          active_election_policy_effects: list(),
          global_events: list(GlobalEvent.t())
        }

  defstruct [
    :war_id,
    :started_at,
    :snapshot_at,
    :impact_multiplier,
    :planet_status,
    :planet_attacks,
    :campaigns,
    :community_targets,
    :joint_operations,
    :planet_events,
    :planet_active_effects,
    :active_election_policy_effects,
    :global_events
  ]

  @spec download(String.t()) :: {:ok, t()} | {:error, term()}
  def download(war_id) do
    default_language = Application.get_env(:helldivers_2, :language)

    translations = :helldivers_2
    |> Application.get_env(:languages)
    |> Task.async_stream(fn {key, lang} -> {key, download_language!(war_id, lang)} end, timeout: :infinity, zip_input_on_exit: true)
    |> Enum.reduce(%{}, fn ({:ok, {key, payload}}, acc) ->
      Map.put(acc, key, payload)
    end)

    # Take the default language as 'base' response object.
    base = Map.get(translations, default_language)

    {:ok, parse(base, translations)}
  end

  @doc """
  Attempts to parse as much information as possible from the given `map` into a struct.
  """
  @spec parse(map(), %{atom() => map()}) :: t()
  def parse(map, translations \\ %{}) when is_map(map) do
    war_id = Map.get(map, "warId")
    snapshot_at = DateTime.utc_now()
    campaigns = Enum.map(Map.get(map, "campaigns"), &Campaign.parse(war_id, &1))

    joint_operations =
      Enum.map(Map.get(map, "jointOperations"), &JointOperation.parse(war_id, &1))

    # We currently only translate global events
    global_event_translations = translations
    |> Map.new(fn {key, payload} -> {key, Map.get(payload, "globalEvents")} end)

    %__MODULE__{
      war_id: war_id,
      started_at: DateTime.add(snapshot_at, - Map.get(map, "time")),
      snapshot_at: snapshot_at,
      impact_multiplier: Map.get(map, "impactMultiplier"),
      planet_status: Enum.map(Map.get(map, "planetStatus"), &PlanetStatus.parse(war_id, &1)),
      planet_attacks:
        Enum.map(Map.get(map, "planetAttacks"), fn attack ->
          {
            WarSeason.get_planet!(war_id, Map.get(attack, "source")),
            WarSeason.get_planet!(war_id, Map.get(attack, "target"))
          }
        end),
      campaigns: campaigns,
      community_targets: [],
      joint_operations: joint_operations,
      planet_events:
        Enum.map(
          Map.get(map, "planetEvents"),
          &PlanetEvent.parse(war_id, campaigns, joint_operations, &1)
        ),
      planet_active_effects: [],
      active_election_policy_effects: [],
      global_events: map
      |> Map.get("globalEvents")
      |> Enum.map(&GlobalEvent.parse(war_id, &1, global_event_translations))
      |> Enum.sort(fn (event1, event2) -> event1.id > event2.id end)
    }
  end

  @spec download_language!(String.t(), String.t()) :: map() | no_return()
  defp download_language!(war_id, language) do
    Logger.debug("Fetching #{language} for #{war_id}")

    response =
      [url: "https://api.live.prod.thehelldiversgame.com/api/WarSeason/#{war_id}/Status", retry: :transient]
      |> Req.new()
      |> Req.Request.put_header("accept-language", language)
      |> Req.request!()

    case response do
      %Req.Response{status: 200} ->
        response.body

      %Req.Response{status: status} ->
        raise "API returned an error #{status}"
    end
  end
end
