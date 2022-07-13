// ReSharper disable StringLiteralTypo
namespace PetsOptimizer;

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
        return $"{TerritoryNames[TerritoryPosition]}\t\tExpected Power: {Math.Round(GetTotalForagePower())}" + $"\n\t{string.Join("\n\t", Pets.Select(p => p.ToString()))}\n";
    }

    //public void Mutate()
    //{
    //    var swappingPet = Population.BreedingData.Pets.Except(Pets).RandomSubset(1);

    //    Population.Territories.Where(t => t != this).Shuffle().First()
    //}

    public double GetTotalForagePower()
    {
        var totalRawPower =
            Pets.Where(p => p.GeneEffect is IForagerGeneEffect).Sum(p => p.Strength) *
            Population.BreedingData.FightContribution +
            Pets.Where(p => p.GeneEffect is IFighterGeneEffect).Select(p =>
                p.GeneEffect is MercenaryEffect effect ? p.Strength * effect.StrengthMultiplier : p.Strength).Sum();

        if (totalRawPower < TerritoryPowerRequirements[TerritoryPosition])
        {
            return 0;
        }

        var usefulPets = Pets.Where(p => p.GeneEffect.DoesMultiplierApplyToForaging(this));

        var petForagingPower = Pets.ToDictionary(p => p, p => p.GeneEffect is IForagerGeneEffect ? p.Strength : 0);

        foreach (var usefulPet in usefulPets)
        {
            switch (usefulPet.GeneEffect.Application)
            {
                case IGeneEffect.GeneApplication.Individual:
                    petForagingPower[usefulPet] *= usefulPet.GeneEffect.StrengthMultiplier;

                    break;

                case IGeneEffect.GeneApplication.All:
                {
                    foreach (var pet in petForagingPower.Keys.ToList())
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