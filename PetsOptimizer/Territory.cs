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
            throw new ArgumentOutOfRangeException(nameof(pets), pets, "Only 4 pets per territory!");
        }

        Pets = petsList;
    }

    public readonly int TerritoryPosition;

    public List<Pet> Pets { get; }

    public Population Population { get; }

    public double GetTotalForagePower()
    {


        throw new NotImplementedException();
    }

    public static List<int> TerritoryPowerRequirements = new List<int>()
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
}