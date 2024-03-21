defmodule Helldivers2.WarSync do
  @moduledoc """
  A GenServer responsible for periodically synchronizing Helldivers 2 API.

  It does this by requesting `Helldivers2.Models.WarInfo.download/1`
  and `Helldivers2.Models.WarStatus.download/1`.

  The resulting information is then sent to the `Helldivers2.WarSeason` process
  responsible for the season being synced.
  """
  alias Helldivers2.Models.Assignment
  alias Helldivers2.Models.NewsFeed
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

  def child_spec(options) do
    Supervisor.child_spec(
      %{
        id: Keyword.get(options, :war_id),
        start: {__MODULE__, :start_link, [options]}
      },
      []
    )
  end

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
    interval = Keyword.get(opts, :interval)
    WarSeason.start_link(war_id: war_id)

    send(self(), :sync)

    {:ok,
     %{
       war_id: war_id,
       interval: interval
     }}
  end

  @impl GenServer
  def handle_info(:sync, %{war_id: war_id, interval: interval} = state) do
    Process.send_after(self(), :sync, interval)

    with {:ok, war_info} <- WarInfo.download(war_id),
         :ok <- WarSeason.store(war_id, war_info),
         {:ok, war_status} <- WarStatus.download(war_id),
         :ok <- WarSeason.store(war_id, war_status),
         {:ok, news_feed} <- NewsFeed.download(war_id),
         :ok <- WarSeason.store(war_id, news_feed, :news_feed),
         {:ok, assignment} <- Assignment.download(war_id),
         :ok <- WarSeason.store(war_id, assignment, :assignment) do
      Logger.info("Finished synchronizing API #{war_id}")
    end

    {:noreply, state, :hibernate}
  end
end
