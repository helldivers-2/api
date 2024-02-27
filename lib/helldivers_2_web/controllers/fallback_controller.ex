defmodule Helldivers2Web.FallbackController do
  @moduledoc """
  Translates controller action results into valid `Plug.Conn` responses.

  See `Phoenix.Controller.action_fallback/1` for more details.
  """

  use Helldivers2Web, :controller

  # This clause handles errors returned by the `PaceVet.Accounts` domain
  def call(conn, {:error, :unauthorized}) do
    conn
    |> put_resp_header("www-authorize", "Bearer")
    |> send_resp(:unauthorized, "")
  end

  # This clause is an example of how to handle requests that are malformed.
  def call(conn, {:error, :unprocessable_entity}) do
    conn
    |> put_status(:unprocessable_entity)
    |> put_view(html: Helldivers2Web.ErrorHTML, json: Helldivers2Web.ErrorJSON)
    |> render(:"422")
  end

  # This clause is an example of how to handle resources that cannot be found.
  def call(conn, {:error, :not_found}) do
    conn
    |> put_status(:not_found)
    |> put_view(html: Helldivers2Web.ErrorHTML, json: Helldivers2Web.ErrorJSON)
    |> render(:"404")
  end
end
