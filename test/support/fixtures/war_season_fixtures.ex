defmodule Helldivers2.WarSeasonFixtures do
  alias Helldivers2.Models.WarStatus.Campaign
  alias Helldivers2.Models.WarStatus.PlanetStatus
  alias Helldivers2.Models.WarInfo.HomeWorld
  alias Helldivers2.Models.WarInfo
  alias Helldivers2.Models.WarInfo.Planet

  def war_info_fixture do
    %WarInfo{
      war_id: 801,
      start_date: ~U[2024-01-23 20:05:13Z],
      end_date: ~U[2028-02-08 20:04:55Z],
      minimum_client_version: "0.3.0",
      planets: [
        planet_super_earth_fixture(),
        planet_klen_dath_fixture()
      ],
      home_worlds: [
        %HomeWorld{
          race: "Humans",
          planets: [
            planet_super_earth_fixture()
          ]
        },
        %HomeWorld{
          race: "Automaton",
          planets: [
            planet_klen_dath_fixture()
          ]
        }
      ],
      capitals: [],
      planet_permanent_effects: []
    }
  end

  def planet_super_earth_fixture do
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
    }
  end

  def planet_klen_dath_fixture do
    %Planet{
      index: 1,
      name: "Klen Dahth II",
      hash: 3_621_417_917,
      position: {0.05373042, 0.10565466},
      waypoints: [2],
      sector: "Altus",
      max_health: 1_000_000,
      disabled: false,
      initial_owner: "Humans"
    }
  end

  def war_status_fixture do
    %Helldivers2.Models.WarStatus{
      war_id: 801,
      started_at: ~U[1970-01-22 10:22:30Z],
      snapshot_at: ~U[1970-01-22 10:22:30Z],
      impact_multiplier: 9.453553e-4,
      planet_status: [
        %PlanetStatus{
          planet: planet_super_earth_fixture(),
          owner: "Humans",
          health: 1_000_000,
          regen_per_second: 138.88889,
          players: 1728,
          liberation: 100
        },
        %PlanetStatus{
          planet: planet_klen_dath_fixture(),
          owner: "Humans",
          health: 1_000_000,
          regen_per_second: 138.88889,
          players: 0,
          liberation: 100
        }
      ],
      planet_attacks: [
        {planet_klen_dath_fixture(), planet_super_earth_fixture()}
      ],
      campaigns: [
        %Campaign{
          id: 49_961,
          planet: planet_super_earth_fixture(),
          type: 0,
          count: 1
        },
        %Helldivers2.Models.WarStatus.Campaign{
          id: 49_985,
          planet: planet_klen_dath_fixture(),
          type: 0,
          count: 1
        }
      ],
      community_targets: [],
      joint_operations: [
        %Helldivers2.Models.WarStatus.JointOperation{
          id: 4292,
          planet: planet_klen_dath_fixture(),
          hq_node_index: 1
        }
      ],
      planet_events: [
        %Helldivers2.Models.WarStatus.PlanetEvent{
          id: 4292,
          planet: planet_klen_dath_fixture(),
          event_type: 1,
          race: "Automaton",
          health: 1_017_958,
          max_health: 2_000_000,
          start_time: ~U[1970-01-21 09:11:00Z],
          expire_time: ~U[1970-01-23 01:11:00Z],
          campaign: %Helldivers2.Models.WarStatus.Campaign{
            id: 49_985,
            planet: planet_super_earth_fixture(),
            type: 0,
            count: 6
          },
          joint_operations: [
            %Helldivers2.Models.WarStatus.JointOperation{
              id: 4292,
              planet: planet_klen_dath_fixture(),
              hq_node_index: 1
            }
          ]
        }
      ],
      planet_active_effects: [],
      active_election_policy_effects: [],
      global_events: [
        %Helldivers2.Models.WarStatus.GlobalEvent{
          id: 1_499_071,
          id_32: 0,
          portrait_id_32: 0,
          title: "SUCCESS",
          title_32: 2_998_873_950,
          message: %{
            "en" =>
              "Well done, Helldivers. SEAF Containment Teams will take over cleanup and inoculation operations from here. Colonists have been advised that there is absolutely zero threat of further Terminid violence, and to settle their families with total peace of mind.\n\nAdditionally, the pilots allocated to spore clearance on Veld have now been redirected to Helldiver support operations. \n<i=1>Extraction will be faster on all operations.</i> ",
            "de" =>
              "Gut gemacht, Helldivers. S.E.A.F.-Sicherungsteams übernehmen ab hier die Säuberungs- und Impfaufgaben. Die Kolonisten wurden darüber informiert, dass absolut keine Gefahr weiterer Bedrohungen durch Terminiden besteht und dass sie sich in aller Ruhe mit ihren Familien niederlassen können.\n\nDarüber hinaus wurden die Piloten, die die Sporen auf Veld beseitigen sollten, nun angewiesen, die Helldivers bei ihren Bemühungen zu unterstützen.\n<i=1>Die Evakuierung geht bei allen Operationen schneller vonstatten.</i>"
          },
          message_id_32: 2_450_763_514,
          race: "Humans",
          flag: 2,
          assignment_id_32: 0,
          effects: [],
          planets: []
        },
        %Helldivers2.Models.WarStatus.GlobalEvent{
          id: 1_499_072,
          id_32: 0,
          portrait_id_32: 0,
          title: "BRIEFING",
          title_32: 2_908_633_975,
          message: %{
            "en" =>
              "Despite the valorous efforts of the Helldivers, Automaton marauders have invaded Super Earth territory. Patriotic citizens mourn as their sufficiently-sized homes burn to the ground.\n\nSuper Earth citizens demand justice, and they will receive it.",
            "de" =>
              "Trotz der mutigen Anstrengungen der Helldivers konnten Roboter-Marodeure in Über-Erde-Gebiete eindringen. Patriotische Bürger verfallen in tiefe Trauer, während ihre ausreichend großen Behausungen niederbrennen.\n\nDie Bürger von Über-Erde verlangen Gerechtigkeit und sie werden sie bekommen."
          },
          message_id_32: 2_667_498_758,
          race: "Humans",
          flag: 1,
          assignment_id_32: 4_098_053_760,
          effects: [],
          planets: []
        }
      ]
    }
  end
end
