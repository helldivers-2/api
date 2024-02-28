defmodule Helldivers2Web.Api.PlanetsControllerTest do
  use Helldivers2Web.ConnCase

  import OpenApiSpex.TestAssertions

  alias Helldivers2.Models.WarInfo.Planet
  alias Helldivers2.Models.WarInfo
  alias Helldivers2.WarSeason

  @war_id "1"

  setup _ do
    # Start up an internal war season
    {:ok, _} = WarSeason.start_link(war_id: @war_id)

    # Store a few planets in the given war season
    WarSeason.store(@war_id, %WarInfo{
      planets: [
        %Planet{
          index: 0,
          name: "Super Earth",
          hash: 897_386_910,
          position: {0, 0},
          waypoints: [1],
          sector: "Sol",
          max_health: 1_000_000,
          disabled: false,
          initial_owner: "Humans"
        },
        %Planet{
          index: 1,
          name: "Klen Dahth II",
          hash: 3621417917,
          position: {0.05373042, 0.10565466},
          waypoints: [2],
          sector: "Altus",
          max_health: 1000000,
          disabled: false,
          initial_owner: "Humans"
        }
      ]
    })

    :ok
  end

  describe "GET /api/:war_id/planets" do
    test "returns a list of all available planets", %{conn: conn} do
      response =
        conn
        |> get(~p"/api/#{@war_id}/planets")
        |> json_response(200)

      assert [%{"index" => 0}] = Enum.take(response, 1)
    end

    test "conforms to it's schema", %{conn: conn} do
      response =
        conn
        |> get(~p"/api/#{@war_id}/planets")
        |> json_response(200)

      spec = Helldivers2Web.ApiSpec.spec()
      for planet <- response do
        assert_schema(planet, "PlanetSchema", spec)
      end
    end
  end

  describe "GET /api/:war_id/planets/:planet_index" do
    test "returns 404 for unknown planets", %{conn: conn} do
      conn
      |> get(~p"/api/#{@war_id}/planets/5")
      |> json_response(404)
    end

    test "returns 422 for invalid planet indices", %{conn: conn} do
      conn
      |> get(~p"/api/#{@war_id}/planets/invalid")
      |> json_response(422)
    end

    test "returns 200 for a valid planet index", %{conn: conn} do
      conn
      |> get(~p"/api/#{@war_id}/planets/0")
      |> json_response(200)
    end

    test "response conforms to schema", %{conn: conn} do
      response =
        conn
        |> get(~p"/api/#{@war_id}/planets/0")
        |> json_response(200)

      spec = Helldivers2Web.ApiSpec.spec()
      assert_schema(response, "PlanetSchema", spec)
    end
  end
end
