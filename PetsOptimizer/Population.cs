namespace PetsOptimizer;

using Genes;

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

    private void PrepareTerritoryMultipliers()
    {
        foreach (var territory in Territories)
        {
            territory.ResetRegionalMultipliers();
        }

        foreach (var territory in Territories)
        {
            foreach (var territoryPet in territory.Pets)
            {
                if (territoryPet.GeneEffect.Application == IGeneEffect.GeneApplication.Regional)
                {
                    var territoryPosition = territory.TerritoryPosition;
                    var multiplier = territoryPet.GeneEffect.StrengthMultiplier;
                    var petEffect = territoryPet.GeneEffect;

                    switch (territoryPosition)
                    {
                        case > 0 when territoryPosition < Territories.Count - 1:
                            AdjustMiddleTerritory(petEffect, territoryPosition, multiplier);

                            break;

                        case 0:
                            AdjustTopTerritory(petEffect, territoryPosition, multiplier);

                            break;

                        case > 0 when territoryPosition == Territories.Count - 1:
                            AdjustBottomTerritory(petEffect, territoryPosition, multiplier);

                            break;
                    }
                }
            }

        }

        void AdjustTerritoryForagingAbove(int territoryPosition, double multiplier)
        {
            Territories[territoryPosition - 1].RegionalForagingMultiplier *= multiplier;
        }

        void AdjustTerritoryForagingBelow(int territoryPosition, double multiplier)
        {
            Territories[territoryPosition + 1].RegionalForagingMultiplier *= multiplier;
        }

        void AdjustTerritoryFightingAbove(int territoryPosition, double multiplier)
        {
            Territories[territoryPosition - 1].RegionalFightingMultiplier *= multiplier;
        }

        void AdjustTerritoryFightingBelow(int territoryPosition, double multiplier)
        {
            Territories[territoryPosition + 1].RegionalFightingMultiplier *= multiplier;
        }

        void AdjustMiddleTerritory(IGeneEffect geneEffect, int territoryPosition, double multiplier)
        {
            switch (geneEffect)
            {
                case IForagerGeneEffect:
                    AdjustTerritoryForagingAbove(territoryPosition, multiplier);
                    AdjustTerritoryForagingBelow(territoryPosition, multiplier);

                    break;

                case IFighterGeneEffect:
                    AdjustTerritoryFightingAbove(territoryPosition, multiplier);
                    AdjustTerritoryFightingBelow(territoryPosition, multiplier);

                    break;
            }
        }

        void AdjustTopTerritory(IGeneEffect geneEffect, int territoryPosition, double multiplier)
        {
            switch (geneEffect)
            {
                case IForagerGeneEffect:
                    AdjustTerritoryForagingBelow(territoryPosition, multiplier);

                    break;

                case IFighterGeneEffect:
                    AdjustTerritoryFightingBelow(territoryPosition, multiplier);

                    break;
            }
        }

        void AdjustBottomTerritory(IGeneEffect geneEffect, int territoryPosition, double multiplier)
        {
            switch (geneEffect)
            {
                case IForagerGeneEffect:
                    AdjustTerritoryForagingAbove(territoryPosition, multiplier);

                    break;

                case IFighterGeneEffect:
                    AdjustTerritoryFightingAbove(territoryPosition, multiplier);

                    break;
            }
        }
    }

    public double GetTotalScore()
    {
        PrepareTerritoryMultipliers();

        double sum = 0;

        for (var i = 0; i < Territories.Count; ++i)
        {
            var territory = Territories[i];

            sum += territory.GetTotalForagePower() * (1 + i * 0.1);
        }

        return sum;
    }

    public void Mutate()
    {
        var removedPets = new List<Pet>();

        foreach (var pet in BreedingData.Pets.RandomSubset(Random.Shared.Next(0, Math.Min(5, BreedingData.PetCount))))
        {
            if (removedPets.Contains(pet))
            {
                continue;
            }

            var foundTerritory = Territories.FirstOrDefault(t => t.Pets.Contains(pet));

            if (foundTerritory == null)
            {
                var randomTerritory = Territories[Random.Shared.Next(Territories.Count)];

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

        file.WriteLine($"Overall power: {Math.Floor(Territories.Sum(t => t.GetTotalForagePower())):n0}\n");

        file.WriteLine(string.Join("\n", Territories.Select(t => t.ToString())));
    }
}