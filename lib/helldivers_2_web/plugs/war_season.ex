defmodule Helldivers2Web.Plugs.WarSeason do
  import Plug.Conn

  alias Helldivers2Web.FallbackController
  alias Helldivers2.WarSeason

  @doc """
  If the request contains a `war_id` it validates that it exists.

  This ensures that the rest of the application can assume any `war_id` passed exists.
  """
  @spec check_war_id(Plug.Conn.t()) :: Plug.Conn.t()
  def check_war_id(conn, _options \\ []) do
    case Map.get(conn.params, "war_id") do
      nil ->
        conn
      war_id ->
        validate_war_id(conn, war_id)
    end
  end

  # We have a war_id, validate it exists
  defp validate_war_id(conn, war_id) do
    case WarSeason.exists?(war_id) do
      true ->
        conn
      false ->
        conn
        |> FallbackController.call({:error, :not_found})
        |> halt()
    end
  end
end
