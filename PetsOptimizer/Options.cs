namespace PetsOptimizer;

using CommandLine;

public class Options
{
    [Option('p', "population", Required = false, HelpText = "Sets the population size", Default = 5000)]
    public int PopulationSize { get; set; }

    [Option('i', "iterations", Required = false, HelpText = "Sets the number of iterations per run", Default = 1500)]
    public int Iterations { get; set; }

    [Option('r', "runs", Required = false, HelpText = "Sets the number of runs", Default = 1)]
    public int Runs { get; set; }

    [Option('f', "file", Required = false, HelpText = "Sets the save data path. Defaults to %appdata%/IdleonSaver/idleon_save.json")]
    public string FilePath { get; set; } =
        Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "IdleonSaver",
            "idleon_save.json");
};