namespace PetsOptimizer;

using JsonParser;

using MoreLinq.Extensions;

public class Population
{
    public Population(BreedingData breedingData)
    {
        BreedingData = breedingData;

        Territories = breedingData.ShuffledPets.Batch(4).Take(breedingData.Territories)
            .Select((batch, i) => new Territory(this, batch, i)).ToList();
    }

    public Population(Population other)
    {
        BreedingData = other.BreedingData;

        Territories = other.Territories.Select(t => new Territory(this, new List<Pet>(t.Pets), t.TerritoryPosition))
            .ToList();

        Mutate();
    }

    public BreedingData BreedingData { get; }

    public List<Territory> Territories { get; }

    public double GetTotalScore()
    {
        return Territories.Select((t, i) => t.GetTotalForagePower() * (1 + i * 0.1)).Sum();
    }

    public void Mutate()
    {
        var removedPets = new List<Pet>();

        foreach (var pet in BreedingData.Pets.RandomSubset(4))
        {
            if (removedPets.Contains(pet))
            {
                continue;
            }

            var foundTerritory = Territories.FirstOrDefault(t => t.Pets.Contains(pet));

            if (foundTerritory == null)
            {
                var randomTerritory = Territories.RandomSubset(1).First();

                var index = Random.Shared.Next(randomTerritory.Pets.Count);

                removedPets.Add(randomTerritory.Pets[index]);

                randomTerritory.Pets[index] = pet;
            }
            else
            {
                var otherRandomTerritory = Territories.Where(t => t != foundTerritory).RandomSubset(1).First();

                var otherIndex = Random.Shared.Next(otherRandomTerritory.Pets.Count);
                var petIndex = foundTerritory.Pets.IndexOf(pet);

                foundTerritory.Pets[petIndex] = otherRandomTerritory.Pets[otherIndex];

                otherRandomTerritory.Pets[otherIndex] = pet;
            }
        }
    }

    public void WriteToFile()
    {
        using var file = File.CreateText("best-result.txt");

        file.WriteLine(string.Join("\n", Territories.Select(t => t.ToString())));
    }
}