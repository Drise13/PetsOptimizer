using System.Diagnostics;

using CommandLine;

using MoreLinq;

using Newtonsoft.Json;

using PetsOptimizer;
using PetsOptimizer.JsonParser;

var parsedArgs = Parser.Default.ParseArguments<Options>(args);

var jsonDataString = File.ReadAllText(parsedArgs.Value.FilePath);

var data = JsonConvert.DeserializeObject<BreedingData>(jsonDataString);

data.Overrides = parsedArgs.Value.Priorities;

IEnumerable<Population> CreatePopulations(int populationCount)
{
    return Enumerable.Range(0, populationCount).Select(_ => new Population(data));
}

const int outputIteration = 10;

var bestPopulations = new List<Population>();

var stopwatch = new Stopwatch();

stopwatch.Start();

var frameTimings = new List<long>(parsedArgs.Value.Iterations * parsedArgs.Value.Runs);

foreach (var r in Enumerable.Range(0, parsedArgs.Value.Runs))
{
    var populations = CreatePopulations(parsedArgs.Value.PopulationSize).ToList();

    var previousBest = -1.0;

    foreach (var i in Enumerable.Range(0, parsedArgs.Value.Iterations))
    {
        populations = populations.AsParallel()
            .OrderByDescending(pop => pop.GetTotalScore())
            .Take(parsedArgs.Value.PopulationSize / 2)
            .ToList();

        populations.Skip(Math.Max(1, parsedArgs.Value.PopulationSize / 10)).AsParallel().ForEach(pop => pop.Mutate());

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

    if (r + 1 != parsedArgs.Value.Runs)
    {
        Console.WriteLine($"Running again... Run number: {r + 1}\n\n");
    }
}

Console.WriteLine($"Average iteration time: {frameTimings.Average() / outputIteration:n0}ms");

bestPopulations.OrderByDescending(pop => pop.GetTotalScore()).First().WriteToFile();