namespace Helldivers.Models.Domain;

public record Faction(
    int Index,
    string Name,
    Planet Homeworld
);
