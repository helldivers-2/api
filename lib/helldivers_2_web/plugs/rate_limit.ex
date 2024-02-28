defmodule Helldivers2Web.Plugs.RateLimit do
  import Phoenix.Controller, only: [json: 2]
  import Plug.Conn

  def rate_limit(conn, options \\ []) do
    interval_milliseconds = options[:interval_seconds] * 1000
    max_requests = options[:max_requests]

    case check_rate(conn, interval_milliseconds, max_requests) do
      {:ok, count} -> put_rate_limit_headers(conn, max_requests, max_requests - count)
      {:error, _count} -> render_error(conn, interval_milliseconds, max_requests)
    end
  end

  defp check_rate(conn, interval, limit) do
    ExRated.check_rate(bucket_name(conn), interval, limit)
  end

  # Bucket name is an IP address
  defp bucket_name(conn) do
    conn.remote_ip |> Tuple.to_list() |> Enum.join(".")
  end

  defp render_error(conn, interval, limit) do
    {_, _, ms_to_next_bucket, _, _} = ExRated.inspect_bucket(bucket_name(conn), interval, limit)

    conn
    |> put_rate_limit_headers(limit, 0)
    |> put_resp_header("retry-after", to_string(ms_to_next_bucket / 1_000))
    |> put_status(:too_many_requests)
    |> json(%{error: "Rate limit exceeded."})
    |> halt()
  end

  defp put_rate_limit_headers(conn, limit, remaining) do
    conn
    |> put_resp_header("x-ratelimit-limit", to_string(limit))
    |> put_resp_header("x-ratelimit-remaining", to_string(remaining))
  end
end
