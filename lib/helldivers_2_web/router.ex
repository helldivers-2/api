defmodule Helldivers2Web.Router do
  use Helldivers2Web, :router

  pipeline :browser do
    plug :accepts, ["html"]
    plug :fetch_session
    plug :fetch_live_flash
    plug :put_root_layout, html: {Helldivers2Web.Layouts, :root}
    plug :protect_from_forgery
    plug :put_secure_browser_headers
  end

  pipeline :api do
    plug :accepts, ["json"]
  end

  # scope "/", Helldivers2Web do
  #   pipe_through :browser

  #   get "/", PageController, :home
  # end

  # Other scopes may use custom stacks.
  scope "/api", Helldivers2Web.Api do
    pipe_through :api

    get "/", WarSeasonController, :index
    get "/:war_id/info", WarSeasonController, :show_info
    get "/:war_id/planets", PlanetsController, :index
  end

  # Enable LiveDashboard in development
  if Application.compile_env(:helldivers_2, :dev_routes) do
    # If you want to use the LiveDashboard in production, you should put
    # it behind authentication and allow only admins to access it.
    # If your application does not have an admins-only section yet,
    # you can use Plug.BasicAuth to set up some basic authentication
    # as long as you are also using SSL (which you should anyway).
    import Phoenix.LiveDashboard.Router

    scope "/dev" do
      pipe_through :browser

      live_dashboard "/dashboard", metrics: Helldivers2Web.Telemetry
    end
  end
end
