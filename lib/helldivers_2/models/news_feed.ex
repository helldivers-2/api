defmodule Helldivers2.Models.NewsFeed do
  alias Helldivers2.Models.NewsFeed.Message

  @type t :: list(Message.t())

  @spec download(String.t()) :: {:error, any()} | {:ok, list(Message.t())}
  def download(war_id) do
    default_language = Application.get_env(:helldivers_2, :language)

    translations = :helldivers_2
    |> Application.get_env(:languages)
    |> Task.async_stream(fn {key, lang} -> {key, download_language!(war_id, lang)} end, timeout: :infinity, zip_input_on_exit: true)
    |> Enum.reduce(%{}, fn ({:ok, {key, payload}}, acc) ->
      Map.put(acc, key, payload)
    end)

    # Take the default language as 'base' response object.
    base = Map.get(translations, default_language)

    {:ok, parse(base, translations)}
  end

  @spec parse(list(map()), list(map())) :: list(Message.t())
  def parse(list, translations) when is_list(list) do
    Enum.map(list, &Message.parse(&1, translations))
  end

  @spec download_language!(String.t(), String.t()) :: map() | no_return()
  defp download_language!(war_id, language) do
    response =
      [
        url: "https://api.live.prod.thehelldiversgame.com/api/NewsFeed/#{war_id}",
        retry: :transient
      ]
      |> Req.new()
      |> Req.Request.put_header("accept-language", language)
      |> Req.request!()

    case response do
      %Req.Response{status: 200} ->
        response.body

      %Req.Response{status: status} ->
        raise "API returned an error #{status}"
    end
  end
end
