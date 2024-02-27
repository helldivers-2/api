defmodule Helldivers2.Application do
  # See https://hexdocs.pm/elixir/Application.html
  # for more information on OTP Applications
  @moduledoc false

  use Application

  @impl true
  def start(_type, _args) do
    children = [
      Helldivers2Web.Telemetry,
      {DNSCluster, query: Application.get_env(:helldivers_2, :dns_cluster_query) || :ignore},
      {Phoenix.PubSub, name: Helldivers2.PubSub},
      {Registry, keys: :unique, name: Helldivers2.WarSync.Registry},
      {Registry, keys: :unique, name: Helldivers2.WarSeason.Registry},
      # Start a sync for war 801
      {Helldivers2.WarSync, war_id: "801"},
      # Start a worker by calling: Helldivers2.Worker.start_link(arg)
      # {Helldivers2.Worker, arg},
      # Start to serve requests, typically the last entry
      Helldivers2Web.Endpoint
    ]

    # See https://hexdocs.pm/elixir/Supervisor.html
    # for other strategies and supported options
    opts = [strategy: :one_for_one, name: Helldivers2.Supervisor]
    Supervisor.start_link(children, opts)
  end

  # Tell Phoenix to update the endpoint configuration
  # whenever the application is updated.
  @impl true
  def config_change(changed, _new, removed) do
    Helldivers2Web.Endpoint.config_change(changed, removed)
    :ok
  end
end
