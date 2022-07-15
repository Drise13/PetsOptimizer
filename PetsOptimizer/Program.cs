using System.Diagnostics;

using MoreLinq;

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
const int iterations = 1300;
const int runs = 1;
const int outputIteration = 10;

var bestPopulations = new List<Population>();

var stopwatch = new Stopwatch();

stopwatch.Start();

var frameTimings = new List<long>(iterations * runs);

foreach (var r in Enumerable.Range(0, runs))
{
    var populations = CreatePopulations(populationSize).ToList();

    var previousBest = -1.0;

    foreach (var i in Enumerable.Range(0, iterations))
    {
        populations = populations.AsParallel()
            .OrderByDescending(pop => pop.GetTotalScore())
            .Take(populationSize / 2)
            .ToList();

        populations.Skip(Math.Max(1, populationSize / 10)).AsParallel().ForEach(pop => pop.Mutate());

        if (i % outputIteration == 0)
        {
            var newBest = Math.Floor(populations.First().GetTotalScore());

            if (newBest < 0)
            {
                previousBest = newBest;
            }

            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            frameTimings.Add(elapsedMilliseconds);

            Console.WriteLine($"{$"Iteration: {i}",-18} {$"Best Score: {newBest:n0}",-25}" +
                              $"{$"{(i > 0 ? Math.Round((newBest - previousBest) / previousBest * 100, 2) : "~")}%",-7} {elapsedMilliseconds}ms");

            previousBest = newBest;

            stopwatch.Restart();
        }

        var newPopulations = populations.AsParallel().Select(pop => new Population(pop)).ToList();

        populations.AddRange(newPopulations);
    }

    bestPopulations.Add(populations.First());

    if (r + 1 != runs)
    {
        Console.WriteLine($"Running again... Run number: {r + 1}\n\n");
    }
}

Console.WriteLine($"Average iteration time: {frameTimings.Average() / outputIteration:n0}ms");

bestPopulations.OrderByDescending(pop => pop.GetTotalScore()).First().WriteToFile();