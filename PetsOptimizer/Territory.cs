// ReSharper disable StringLiteralTypo
namespace PetsOptimizer;

using Genes;

public class Territory
{
    public Territory(Population parentPopulation, IEnumerable<Pet> pets, int territoryPosition)
    {
        Population = parentPopulation;
        TerritoryPosition = territoryPosition;

        var petsList = pets.ToList();

        if (petsList.Count > 4)
        {
            throw new ArgumentOutOfRangeException(nameof(pets), pets, "Only 4 pets max per territory!");
        }

        Pets = petsList;
    }

    public readonly int TerritoryPosition;

    public List<Pet> Pets { get; }

    public Population Population { get; }

    public override string ToString()
    {
        return $"{TerritoryNames[TerritoryPosition]}\t\tExpected Power: {Math.Floor(GetTotalForagePower())}" + $"\n\t{string.Join("\n\t", Pets.Select(p => p.ToString()))}\n";
    }

    public double GetTotalForagePower()
    {
        var totalRawPower = 0.0;

        foreach (var pet in Pets)
        {
            switch (pet.GeneEffect)
            {
                case MercenaryEffect mercenaryEffect:
                    totalRawPower += pet.Strength * mercenaryEffect.StrengthMultiplier;

                    break;

                case IFighterGeneEffect:
                    totalRawPower += pet.Strength;

                    break;

                case IForagerGeneEffect:
                    totalRawPower += pet.Strength * Population.BreedingData.FightContribution;

                    break;
            }
        }

        if (totalRawPower < TerritoryPowerRequirements[TerritoryPosition])
        {
            return 0;
        }

        var usefulPets = Pets.Where(p => p.GeneEffect.DoesMultiplierApplyToForaging(this));

        var petForagingPower = Pets.ToDictionary(pet => pet, pet => pet.GeneEffect is IForagerGeneEffect ? pet.Strength : 0);

        foreach (var usefulPet in usefulPets)
        {
            switch (usefulPet.GeneEffect.Application)
            {
                case IGeneEffect.GeneApplication.Individual:
                    petForagingPower[usefulPet] *= usefulPet.GeneEffect.StrengthMultiplier;

                    break;

                case IGeneEffect.GeneApplication.All:
                {
                    foreach (var pet in petForagingPower.Keys)
                    {
                        petForagingPower[pet] *= usefulPet.GeneEffect.StrengthMultiplier;
                    }

                    break;
                }
            }
        }

        return petForagingPower.Values.Sum();
    }

    public static readonly List<int> TerritoryPowerRequirements = new List<int>
    {
        0,
        5,
        20,
        50,
        100,
        250,
        600,
        1100,
        1750,
        3000,
        5000,
        10000,
        25000,
        40000,
        100000,
        175000,
        300000
    };

    public static readonly List<string> TerritoryNames = new List<string>
    {
        "Grasslands",
        "Jungle",
        "Encroaching Forest",
        "Tree Interior",
        "Stinky Sewers",
        "Desert Oasis",
        "Beach Docks",
        "Coarse Mountains",
        "Twilight Desert",
        "The Crypt",
        "Frosty Peaks",
        "Tundra Outback",
        "Crystal Caverns",
        "Pristalle Lake",
        "Nebulon Mantle",
        "Starfield Skies",
        "Shores of Eternity",
    };
}