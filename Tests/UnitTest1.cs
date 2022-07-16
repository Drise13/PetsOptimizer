namespace Tests;

using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;

using PetsOptimizer;
using PetsOptimizer.Genes;
using PetsOptimizer.JsonParser;

using Xunit;

public class UnitTest1
{
    [Fact]
    public void ValidateMiasmaEffect_TrueIfInCorrectRange()
    {
        // This is super hacky and I hate it but I needed a quick test
        var jsonDataString = File.ReadAllText(new Options().FilePath);

        var data = JsonConvert.DeserializeObject<BreedingData>(jsonDataString);

        var territory = new Territory(new Population(data),
            new[]
            {
                new Pet(new PetData
                {
                    Genetics = PetGenetics.Opticular,
                    Species = Species.Mallay,
                    Strength = 8298
                }),
                new Pet(new PetData
                {
                    Genetics = PetGenetics.Sniper,
                    Species = Species.WildBoar,
                    Strength = 7340
                }),
                new Pet(new PetData
                {
                    Genetics = PetGenetics.Fastidious,
                    Species = Species.Tyson,
                    Strength = 7717
                }),
                new Pet(new PetData
                {
                    Genetics = PetGenetics.Miasma,
                    Species = Species.WodeBoard,
                    Strength = 2637
                })
            }, 0);

        Assert.InRange(territory.GetTotalForagePower(), 211000, 213000);
    }

    [Fact]
    public void ValidateBadumdumEffect_TrueIfInCorrectRange()
    {
        // This is super hacky and I hate it but I needed a quick test
        var jsonDataString = File.ReadAllText(new Options().FilePath);

        var data = JsonConvert.DeserializeObject<BreedingData>(jsonDataString);

        var population = new Population(data);

        population.Territories.Clear();

        population.Territories.Add(new Territory(population,
            new[]
            {
                new Pet(new PetData
                {
                    Genetics = PetGenetics.Miasma,
                    Species = Species.WodeBoard,
                    Strength = 3846
                }),
                new Pet(new PetData
                {
                    Genetics = PetGenetics.Opticular,
                    Species = Species.Mallay,
                    Strength = 8298
                }),
                new Pet(new PetData
                {
                    Genetics = PetGenetics.Forager,
                    Species = Species.Mafioso,
                    Strength = 3853
                }),
                new Pet(new PetData
                {
                    Genetics = PetGenetics.Fleeter,
                    Species = Species.BoredBean,
                    Strength = 7347
                })
            }, 0));

        population.Territories.Add(new Territory(population,
            new[]
            {
                new Pet(new PetData
                {
                    Genetics = PetGenetics.Badumdum,
                    Species = Species.Whale,
                    Strength = 6288
                }),
                new Pet(new PetData
                {
                    Genetics = PetGenetics.Badumdum,
                    Species = Species.Whale,
                    Strength = 7962
                }),
                new Pet(new PetData
                {
                    Genetics = PetGenetics.Fleeter,
                    Species = Species.Crabcake,
                    Strength = 1537
                }),
                new Pet(new PetData
                {
                    Genetics = PetGenetics.Badumdum,
                    Species = Species.Whale,
                    Strength = 6564
                })
            }, 1));

        population.Territories.Add(new Territory(population,
            new[]
            {
                new Pet(new PetData
                {
                    Genetics = PetGenetics.Mercenary,
                    Species = Species.Carrotman,
                    Strength = 2601
                }),
                new Pet(new PetData
                {
                    Genetics = PetGenetics.Fastidious,
                    Species = Species.Tyson,
                    Strength = 8451
                }),
                new Pet(new PetData
                {
                    Genetics = PetGenetics.Opticular,
                    Species = Species.Mallay,
                    Strength = 9149
                }),
                new Pet(new PetData
                {
                    Genetics = PetGenetics.Miasma,
                    Species = Species.WodeBoard,
                    Strength = 4926
                })
            }, 2));

        population.GetTotalScore();

        Assert.InRange(population.Territories[0].GetTotalForagePower(), 393000, 395000);
        Assert.InRange(population.Territories[1].GetTotalForagePower(), 50000, 51000);
        Assert.InRange(population.Territories[2].GetTotalForagePower(), 423000, 425000);
    }
}