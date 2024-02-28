defmodule Helldivers2Web.Api.WarSeasonController do
  use Helldivers2Web, :controller
  alias Helldivers2.WarSeason

  action_fallback Helldivers2Web.FallbackController

  def index(conn, _) do
    json(conn, Application.get_env(:helldivers_2, :war_seasons))
  end

  def show_info(conn, %{"war_id" => war_id}) do
    with {:ok, war_info} <- WarSeason.get_war_info(war_id) do
      render(conn, :show, war_info: war_info)
    end
  end
end
