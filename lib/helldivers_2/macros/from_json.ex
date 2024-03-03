defmodule Helldivers2.Macros.FromJson do
  @moduledoc """
  Macro that generates mappings from JSON files.

  This method generates a `parse/1` method that returns the value.
  This allows constant time lookups from the JSON.
  """

  defmacro __using__(filename) do
    mappings =
      :helldivers_2
      |> :code.priv_dir()
      |> Path.join(filename)
      |> File.read!()
      |> Jason.decode!()

    for {key, value} <- mappings do
      if String.length(value) > 0 do
        quote do
          defp lookup("#{unquote(key)}"), do: unquote(value)
        end
      else
        quote do
          defp lookup("#{unquote(key)}"), do: unquote(key)
        end
      end
    end ++
      [
        quote do
          def all(), do: unquote(Macro.escape(Map.values(mappings)))
        end
      ]
  end
end
