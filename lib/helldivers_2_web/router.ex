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
    plug OpenApiSpex.Plug.PutApiSpec, module: Helldivers2Web.ApiSpec
    plug :rate_limit
    plug :check_war_id
  end

  pipeline :openapi do
    plug :accepts, ["json"]
    plug OpenApiSpex.Plug.PutApiSpec, module: Helldivers2Web.ApiSpec
  end

  # scope "/", Helldivers2Web do
  #   pipe_through :browser

  #   get "/", PageController, :home
  # end

  scope "/api" do
    pipe_through :openapi

    get "/openapi", OpenApiSpex.Plug.RenderSpec, []
    get "/swaggerui", OpenApiSpex.Plug.SwaggerUI, path: "/api/openapi"
  end

  # Other scopes may use custom stacks.
  scope "/api", Helldivers2Web.Api do
    pipe_through :api

    get "/", WarSeasonController, :index
    get "/:war_id/assignments", WarSeasonController, :show_assignments
    get "/:war_id/info", WarSeasonController, :show_info
    get "/:war_id/status", WarSeasonController, :show_status

    get "/:war_id/events", GlobalEventsController, :index
    get "/:war_id/events/latest", GlobalEventsController, :latest

    get "/:war_id/planets", PlanetsController, :index
    get "/:war_id/planets/:planet_index", PlanetsController, :show
    get "/:war_id/planets/:planet_index/status", PlanetsController, :show_planet_status

    get "/:war_id/feed", WarSeasonController, :news_feed
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
