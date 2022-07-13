namespace PetsOptimizer;

using MoreLinq.Extensions;

public class Population
{
    public Population(BreedingData breedingData)
    {
        BreedingData = breedingData;

        Territories  = breedingData.Pets.Shuffle().Batch(4).Select((batch, i) => new Territory(this, batch, i)).ToList();
    }

    public BreedingData BreedingData { get; }

    public List<Territory> Territories { get; }

    public double GetTotalScore()
    {
        return Territories.Select((t, i) => t.GetTotalForagePower() * (1 + i * 0.1)).Sum();
    }
}