defmodule Helldivers2Web.Api.PlanetsController do
  use Helldivers2Web, :controller

  alias Helldivers2.WarSeason

  def index(%Plug.Conn{assigns: %{war_id: war_id}} = conn, _) do
    with {:ok, planets} <- WarSeason.get_planets(war_id) do
      render(conn, :index, planets: planets)
    end
  end
end
