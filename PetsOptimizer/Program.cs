using Newtonsoft.Json;

using PetsOptimizer;
using PetsOptimizer.JsonParser;

var jsonDataString = File.ReadAllText("C:\\Users\\Drise\\cleanBreedingData.json");

var jsonData = JsonConvert.DeserializeObject<BreedingData>(jsonDataString);

IEnumerable<Population> CreatePopulations(int populationCount)
{
    return Enumerable.Range(0, populationCount).Select(_ => new Population(jsonData));
}

const int populationSize = 2000;
const int iterations = 1000;

var populations = CreatePopulations(populationSize).ToList();

var previousBest = -1.0;

foreach (var i in Enumerable.Range(0, iterations))
{
    populations = populations.Select(pop => (pop, totalScore: pop.GetTotalScore()))
        .OrderByDescending(pop => pop.totalScore).Select(pop => pop.pop).Take(populationSize / 2).ToList();

    if (i % 10 == 0)
    {
        var newBest = Math.Round(populations.First().GetTotalScore());

        if (newBest == -1)
        {
            previousBest = newBest;
        }

        Console.WriteLine($"Current iteration: {i} \t\t Best Score: {newBest} {Math.Round((newBest - previousBest) / previousBest * 100, 2)}%");

        previousBest = newBest;
    }

    var newPopulations = populations.Select(pop => new Population(pop)).ToList();

    populations.AddRange(newPopulations);
}

var bestPopulation = populations.First();

Console.WriteLine(bestPopulation.GetTotalScore());

bestPopulation.WriteToFile();