defmodule Helldivers2.Models.Assignment do
  alias Helldivers2.Models.Assignment.Message

  @type t :: list(Message.t())

  @spec download(String.t()) :: {:error, any()} | {:ok, list(Message.t())}
  def download(war_id) do
    base = do_download!(war_id)
    {:ok, parse(base)}
  end

  @spec parse(list(map())) :: list(Message.t())
  def parse(list) when is_list(list) do
    Enum.map(list, &Message.parse(&1))
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
