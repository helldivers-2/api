defmodule Helldivers2Web.Schemas.AssignmentsMessageSchema do
  require OpenApiSpex

  alias OpenApiSpex.Schema

  @languages Application.compile_env(:helldivers_2, :languages)

  @doc "Generates a schema for a single assignment response"
  def response(),
    do:
      {"Assignment response", "application/json", __MODULE__,
       Helldivers2Web.ApiSpec.default_options()}

  def responses(),
    do:
      {"Assignments response", "application/json", %Schema{type: :array, items: __MODULE__},
       Helldivers2Web.ApiSpec.default_options()}

  OpenApiSpex.schema(%{
    description: "Represents an assignment in a list of assignments",
    type: :object,
    properties: %{
      id32: %Schema{
        type: :integer,
        description: "The identifier of this campaign"
      },
      progress: %Schema{
        type: :string,
        format: :"date-time",
        description: "Progress of the assignment. Suspected that it's a tuple of length equal to the checkboxes in the UI. 0 is uncompleted and 1 is completed?"
      },
      expiresIn: %Schema{
        type: :integer,
        description: "When the assignment expires. Probably in seconds."
      },
      setting: %Schema{
        type: :object,
        properties: %{
          type: %Schema{
            type: :integer,
            description: "TODO unknown"
          },
          overrideTitle: %Schema{
            type: :string,
            description: "The title of the assignment. \"MAJOR ORDER\" is one option.",
          },
          overrideBrief: %Schema{
            type: :string,
            description: "Thematic text for what is happening.",
          },
          taskDescription: %Schema{
            type: :string,
            description: "The specific task to perform.",
          },
          tasks: %Schema{
            type: :array,
            items: %Schema{
              type: :object,
              properties: %{
                type: %Schema{
                  type: :integer,
                  description: "Probably keyed to some task string.",
                },
                values: %Schema{
                  type: :array,
                  items: %Schema{
                    type: :integer,
                  }
                },
                valuesTypes: %Schema{
                  type: :array,
                  items: %Schema{
                    type: :integer,
                  }
                },
              },
            },
          },
          reward: %Schema{
            type: :object,
            properties: %{
              amount: %Schema{
                type: :integer,
                description: "",
              },
              id32: %Schema{
                type: :integer,
                description: "",
              },
              type: %Schema{
                type: :integer,
                description: "",
              },
            },
          },
          flags: %Schema{
            type: :integer,
            description: "TODO what is this.",
          }
        }
      },
    }
  })
end

