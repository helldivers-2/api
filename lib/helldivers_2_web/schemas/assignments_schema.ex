defmodule Helldivers2Web.Schemas.AssignmentsMessageSchema do
  require OpenApiSpex

  alias OpenApiSpex.Schema

  @languages Application.compile_env(:helldivers_2, :languages)

  @doc "Generates a schema for a single newsfeed message response"
  def response(),
    do:
      {"Assignment message response", "application/json", __MODULE__,
       Helldivers2Web.ApiSpec.default_options()}

  def responses(),
    do:
      {"Assignments response", "application/json", %Schema{type: :array, items: __MODULE__},
       Helldivers2Web.ApiSpec.default_options()}

  OpenApiSpex.schema(%{
    description: "Represents an assignment in a list of assignments",
    type: :object,
    properties: %{
      id: %Schema{
        type: :integer,
        description: "The identifier of this campaign"
      },
      published: %Schema{
        type: :string,
        format: :"date-time",
        description: "When this message was published"
      },
      type: %Schema{
        type: :integer,
        description:
          "A type identifier, haven't figured out what they mean (seems to be 0 mostly)"
      },
      tag_ids: %Schema{
        type: :array,
        items: %Schema{
          type: :integer,
          description: "Tag identifiers, always empty so no idea what they mean"
        }
      },
      message: %Schema{
        type: :object,
        properties:
          Map.new(@languages, fn {key, lang} ->
            {key,
             %Schema{
               type: :string,
               description: "The message from Super Earth about the news in #{lang}"
             }}
          end)
      }
    }
  })
end

