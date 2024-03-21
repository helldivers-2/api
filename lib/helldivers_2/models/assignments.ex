defmodule Helldivers2.Models.Assignments do
  alias Helldivers2.Models.Assignments.Assignment

  @type t :: list(Assignment.t())

  @spec download(String.t()) :: {:error, any()} | {:ok, list(Assignment.t())}
  def download(war_id) do
    base = do_download!(war_id)
    {:ok, parse(base)}
  end

  @spec parse(list(map())) :: list(Assignment.t())
  def parse(list) when is_list(list) do
    Enum.map(list, &Assignment.parse(&1))
  end

  @spec do_download!(String.t()) :: map() | no_return()
  defp do_download!(war_id) do
    response =
      [
        url: "https://api.live.prod.thehelldiversgame.com/api/v2/Assignment/War/#{war_id}",
        retry: :transient
      ]
      |> Req.new()
      |> Req.request!()

    case response do
      %Req.Response{status: 200} ->
        response.body

      %Req.Response{status: status} ->
        raise "API returned an error #{status}"
    end
  end
end
