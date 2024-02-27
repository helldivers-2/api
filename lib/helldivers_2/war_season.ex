defmodule Helldivers2.WarSeason do
  @moduledoc """
  Represents a war season, currently there is only one active war season: 801.

  This module encapsulates the state for a given warseason and is responsible
  for storing and retrieving this information.
  """
  use GenServer
  require Logger
  alias Helldivers2.Models.WarStatus.PlanetStatus
  alias Helldivers2.Models.WarInfo.Planet
  alias Helldivers2.Models.WarStatus
  alias Helldivers2.Models.WarInfo

  @options [
    war_id: [
      type: :string,
      required: true
    ]
  ]

  @doc "Supported options:\n#{NimbleOptions.docs(@options)}"
  def start_link(opts) do
    with {:ok, options} <- NimbleOptions.validate(opts, @options) do
      war_id = Keyword.get(opts, :war_id)

      GenServer.start_link(__MODULE__, options,
        name: {:via, Registry, {__MODULE__.Registry, war_id}}
      )
    end
  end

  @spec store(String.t(), WarInfo.t() | WarStatus.t()) :: :ok | :error
  def store(war_id, data) do
    GenServer.call({:via, Registry, {__MODULE__.Registry, war_id}}, {:store, data})
  end

  @doc """
  Lookup the `WarInfo` associated with the given `war_id`.
  """
  @spec get_war_info(String.t()) :: {:ok, WarInfo.t()} | {:error, term()}
  def get_war_info(war_id) do
    case :ets.lookup(table_name(war_id), WarInfo) do
      [] ->
        {:error, :not_found}

      [{WarInfo, war_info}] ->
        {:ok, war_info}
    end
  end

  @doc """
  Lookup the `Planet` associated with the given `war_id` and planet index.
  """
  @spec get_planet(String.t(), non_neg_integer()) :: {:ok, Planet.t()} | {:error, term()}
  def get_planet(war_id, planet_index) do
    case :ets.lookup(table_name(war_id), {Planet, planet_index}) do
      [] ->
        {:error, :not_found}

      [{{Planet, _}, planet}] ->
        {:ok, planet}
    end
  end

  @doc """
  Same as `get_planet/2` but raises on error.
  """
  @spec get_planet!(String.t(), non_neg_integer()) :: Planet.t() | no_return()
  def get_planet!(war_id, planet_index) do
    case get_planet(war_id, planet_index) do
      {:ok, planet} ->
        planet

      {:error, reason} ->
        raise reason
    end
  end

  @doc """
  Lookup all `Planet` associated with the given `war_id`.
  """
  @spec get_planets(String.t()) :: {:ok, list(Planet.t())} | {:error, term()}
  def get_planets(war_id) do
    case :ets.lookup(table_name(war_id), Planet) do
      [] ->
        {:error, :not_found}

      [{_, planets}] ->
        {:ok, planets}
    end
  end

  @doc """
  Lookup the `WarStatus` associated with the given `war_id`.
  """
  @spec get_war_status(String.t()) :: {:ok, WarStatus.t()} | {:error, term()}
  def get_war_status(war_id) do
    case :ets.lookup(table_name(war_id), WarStatus) do
      [] ->
        {:error, :not_found}

      [{WarInfo, war_info}] ->
        {:ok, war_info}
    end
  end

  @spec get_planet_status(String.t(), non_neg_integer()) :: {:ok, PlanetStatus.t()} | {:error, term()}
  def get_planet_status(war_id, planet_index) do
    case :ets.lookup(table_name(war_id), {PlanetStatus, planet_index}) do
      [] ->
        {:error, :not_found}

      [{{PlanetStatus, _}, planet_status}] ->
        {:ok, planet_status}
    end
  end

  @impl GenServer
  def init(opts) do
    war_id = Keyword.get(opts, :war_id)
    Logger.info("Launching war season #{war_id}")

    # First time we have to 'create' the atom.
    name = :"#{__MODULE__}.#{war_id}"
    table = :ets.new(name, [:set, :protected, :named_table])
    {:ok, %{table: table}}
  end

  @impl GenServer
  def handle_call({:store, %WarInfo{} = info}, _from, %{table: table} = state) do
    :ets.insert(table, {WarInfo, info})
    :ets.insert(table, {Planet, info.planets})

    for planet <- info.planets do
      :ets.insert(table, {{Planet, planet.index}, planet})
    end

    {:reply, :ok, state}
  end

  @impl GenServer
  def handle_call({:store, %WarStatus{} = status}, _from, %{table: table} = state) do
    :ets.insert(table, {WarStatus, status})
    :ets.insert(table, {PlanetStatus, status.planet_status})

    for planet_status <- status.planet_status do
      :ets.insert(table, {{PlanetStatus, planet_status.planet.index}, planet_status})
    end

    {:reply, :ok, state}
  end

  # Get the table name for a given war id
  def table_name(war_id) when is_number(war_id), do: table_name(to_string(war_id))

  def table_name(war_id) when is_binary(war_id),
    do: String.to_existing_atom("#{__MODULE__}.#{war_id}")
end
