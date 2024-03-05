defmodule Helldivers2Web.Plugs.RateLimit do
  import Phoenix.Controller, only: [json: 2]
  import Plug.Conn

  @doc """
  Checks if the current `Plug.Conn` is allowed to execute or if it hit it's rate limit.
  """
  @spec rate_limit(Plug.Conn.t(), any()) :: Plug.Conn.t()
  def rate_limit(conn, _) do
    options = Application.get_env(:helldivers_2, Helldivers2Web.Plugs.RateLimit)
    interval_milliseconds = Keyword.get(options, :interval_seconds, 1) * 1000
    max_requests = Keyword.get(options, :max_requests, 1)

    case check_rate(conn, interval_milliseconds, max_requests) do
      {:ok, count, until_expire} ->
        put_rate_limit_headers(conn, max_requests, count, until_expire)

      {:error, _count, until_expire} ->
        render_error(conn, max_requests, until_expire)
    end
  end

  # If the limit is zero we simply return OK for all rate limits
  @spec check_rate(Plug.Conn.t(), non_neg_integer(), non_neg_integer()) ::
          {:ok, count :: non_neg_integer(), until_expiration :: non_neg_integer()}
          | {:error, non_neg_integer(), non_neg_integer()}
  defp check_rate(_, _, 0), do: {:ok, :infinity, 0}

  defp check_rate(conn, interval, limit) do
    name = bucket_name(conn)

    # First check rate limit (which reduces the available count)
    check = ExRated.check_rate(name, interval, limit)

    # Check how much we have left so we can return this information to the client
    {_, _, until_expiration, _, _} = ExRated.inspect_bucket(name, interval, limit)

    case check do
      {:ok, count} ->
        # Count is amount of calls to this bucket, we just want to see the 'remaining'
        {:ok, max(limit - count, 0), until_expiration}

      {:error, _} ->
        {:error, limit, until_expiration}
    end
  end

  # Bucket name is an IP address
  defp bucket_name(conn) do
    conn.remote_ip |> Tuple.to_list() |> Enum.join(".")
  end

  # Rate limit was hit, halt the conn and put response headers
  defp render_error(conn, limit, until_expire) do
    conn
    |> put_rate_limit_headers(limit, 0, until_expire)
    |> put_resp_header("retry-after", to_string(until_expire / 1_000))
    |> put_status(:too_many_requests)
    |> json(%{error: "Rate limit exceeded."})
    |> halt()
  end

  defp put_rate_limit_headers(conn, limit, remaining, until_expire) do
    # Calculate when the bucket will expire (with 100ms buffer)
    reset_at = :erlang.system_time(:millisecond) + until_expire + 100

    conn
    |> put_resp_header("x-ratelimit-limit", to_string(limit))
    |> put_resp_header("x-ratelimit-remaining", to_string(remaining))
    |> put_resp_header("x-ratelimit-reset", to_string(reset_at))
  end
end
