defmodule Helldivers2Web.Api.CampaignJSON do
  alias Helldivers2Web.Api.PlanetsJSON
  alias Helldivers2.Models.WarStatus.Campaign

  def show(%Campaign{} = campaign) do
    %{
      "id" => campaign.id,
      "planet" => PlanetsJSON.show(campaign.planet),
      "type" => campaign.type,
      "count" => campaign.count
    }
  end
end
