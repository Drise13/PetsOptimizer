using System.Diagnostics;

using Newtonsoft.Json;

using PetsOptimizer;
using PetsOptimizer.JsonParser;

var jsonDataString = File.ReadAllText(Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
    "IdleonSaver", "idleon_save.json"));

var data = JsonConvert.DeserializeObject<BreedingData>(jsonDataString);

IEnumerable<Population> CreatePopulations(int populationCount)
{
    return Enumerable.Range(0, populationCount).Select(_ => new Population(data));
}

const int populationSize = 5000;
const int iterations = 1000;

var populations = CreatePopulations(populationSize).ToList();

var previousBest = -1.0;

var stopwatch = new Stopwatch();

stopwatch.Start();

var frameTimings = new List<long>(iterations);

foreach (var i in Enumerable.Range(0, iterations))
{
    populations = populations.AsParallel().OrderByDescending(pop => pop.GetTotalScore()).Take(populationSize / 2)
        .ToList();

    if (i % 10 == 0)
    {
        var newBest = Math.Floor(populations.First().GetTotalScore());

        if (newBest < 0)
        {
            previousBest = newBest;
        }

        var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
        frameTimings.Add(elapsedMilliseconds);

        Console.WriteLine(
            $"Iteration: {i} \t\t Best Score: {newBest} \t{Math.Round((newBest - previousBest) / previousBest * 100, 2)}%\t\t {elapsedMilliseconds}ms");

        previousBest = newBest;

        stopwatch.Restart();
    }

    var newPopulations = populations.AsParallel().Select(pop => new Population(pop)).ToList();

    populations.AddRange(newPopulations);
}

var bestPopulation = populations.First();

Console.WriteLine($"Average frame time: {frameTimings.Average()}ms");

bestPopulation.WriteToFile();