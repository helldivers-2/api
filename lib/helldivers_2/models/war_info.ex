defmodule Helldivers2.Models.WarInfo do
  @moduledoc """
  Represents the information retrieved from the Helldivers 2 API.
  """
  alias Helldivers2.Models.WarInfo.Planet
  alias Helldivers2.Models.WarInfo.HomeWorld

  @type t :: %__MODULE__{
          war_id: String.t(),
          start_date: DateTime.t(),
          end_date: DateTime.t(),
          minimum_client_version: String.t(),
          planets: list(Planet.t()),
          home_worlds: list(HomeWorld.t()),
          capitals: [],
          planet_permanent_effects: []
        }

  defstruct [
    :war_id,
    :start_date,
    :end_date,
    :minimum_client_version,
    :planets,
    :home_worlds,
    :capitals,
    :planet_permanent_effects
  ]

  @spec download(String.t()) :: {:ok, t()} | {:error, term()}
  def download(war_id) do
    with {:ok, response} <-
           Req.get("https://api.live.prod.thehelldiversgame.com/api/WarSeason/#{war_id}/WarInfo"),
         %Req.Response{status: 200, body: payload} <- response do
      {:ok, parse(payload)}
    else
      %Req.Response{status: status} ->
        {:error, "API error #{status}"}
    end
  end

  @doc """
  Attempts to parse as much information as possible from the given `map` into a struct.
  """
  @spec parse(map()) :: t()
  def parse(map) when is_map(map) do
    planets = Enum.map(Map.get(map, "planetInfos"), &Planet.parse/1)

    %__MODULE__{
      war_id: Map.get(map, "warId"),
      start_date: parse_datetime(map, "startDate"),
      end_date: parse_datetime(map, "endDate"),
      minimum_client_version: Map.get(map, "minimumClientVersion"),
      planets: planets,
      home_worlds: Enum.map(Map.get(map, "homeWorlds"), &HomeWorld.parse(&1, planets)),
      capitals: [],
      planet_permanent_effects: []
    }
  end

  @spec parse_datetime(map(), String.t()) :: DateTime.t()
  defp parse_datetime(map, key) do
    with epoch when is_number(epoch) <- Map.get(map, key),
         {:ok, datetime} <- DateTime.from_unix(epoch, :second) do
      datetime
    else
      _ -> DateTime.from_unix!(0, :second)
    end
  end
end
