defmodule Helldivers2Web.Api.WarSeasonControllerTest do
  use Helldivers2Web.ConnCase

  import OpenApiSpex.TestAssertions
  import Helldivers2.WarSeasonFixtures

  alias Helldivers2.WarSeason

  @war_id "1"

  setup _ do
    # Start up an internal war season
    {:ok, _} = WarSeason.start_link(war_id: @war_id)

    WarSeason.store(@war_id, war_info_fixture())
    WarSeason.store(@war_id, war_status_fixture())

    :ok
  end

  describe "GET /api/:war_id/info" do
    test "returns the current war info", %{conn: conn} do
      response = conn
      |> get(~p"/api/#{@war_id}/info")
      |> json_response(200)

      assert %{
        "war_id" => _,
        "start_date" => _,
        "end_date" => _,
        "minimum_client_version" => _,
        "planets" => _,
        "home_worlds" => _,
        "capitals" => _,
        "planet_permanent_effects" => _
      } = response
    end

    test "conforms to the schema", %{conn: conn} do
      response = conn
      |> get(~p"/api/#{@war_id}/info")
      |> json_response(200)

      spec = Helldivers2Web.ApiSpec.spec()
      assert_schema(response, "WarInfoSchema", spec)
    end
  end

  describe "GET /api/:war_id/status" do
    test "returns the current war status", %{conn: conn} do
      assert %{"war_id" => _} = conn
      |> get(~p"/api/#{@war_id}/status")
      |> json_response(200)
    end

    test "conforms to the schema", %{conn: conn} do
      response = conn
      |> get(~p"/api/#{@war_id}/status")
      |> json_response(200)

      spec = Helldivers2Web.ApiSpec.spec()
      assert_schema(response, "WarStatusSchema", spec)
    end
  end
end
