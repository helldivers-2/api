defmodule Helldivers2.WarSync do
  @moduledoc """
  A GenServer responsible for periodically synchronizing Helldivers 2 API.

  It does this by requesting `Helldivers2.Models.WarInfo.download/1`
  and `Helldivers2.Models.WarStatus.download/1`.

  The resulting information is then sent to the `Helldivers2.WarSeason` process
  responsible for the season being synced.
  """
  alias Helldivers2.Models.WarStatus
  alias Helldivers2.Models.WarInfo
  alias Helldivers2.WarSeason

  use GenServer
  require Logger

  @options [
    war_id: [
      type: :string,
      required: true
    ],
    interval: [
      type: :non_neg_integer,
      # 5 minutes
      default: 300_000
    ]
  ]

  @type state :: %{
          war_id: String.t(),
          interval: non_neg_integer(),
          healthy?: boolean(),
          last_error: String.t() | nil,
          last_sync: DateTime.t() | nil,
          war_info: WarInfo.t() | nil
        }

  @doc "Supported options:\n#{NimbleOptions.docs(@options)}"
  def start_link(opts) do
    with {:ok, options} <- NimbleOptions.validate(opts, @options) do
      war_id = Keyword.get(opts, :war_id)

      GenServer.start_link(__MODULE__, options,
        name: {:via, Registry, {__MODULE__.Registry, war_id}}
      )
    end
  end

  @impl GenServer
  def init(opts) do
    war_id = Keyword.get(opts, :war_id)
    WarSeason.start_link(war_id: war_id)

    send(self(), :sync)
    {:ok, Map.new(opts)}
  end

  @impl GenServer
  def handle_info(:sync, %{war_id: war_id} = state) do
    with {:ok, war_info} <- WarInfo.download(war_id),
         :ok <- WarSeason.store(war_id, war_info),
         {:ok, war_status} <- WarStatus.download(war_id),
         :ok <- WarSeason.store(war_id, war_status) do
      Logger.info("Finished synchronizing API #{war_id}")
    end

    {:noreply, state}
  end
end
