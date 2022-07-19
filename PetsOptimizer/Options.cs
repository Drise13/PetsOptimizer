namespace PetsOptimizer;

using System.Text.RegularExpressions;

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

    [Option('o', "override", Required = false, HelpText = "A comma separated list (starting with 1) of territories to prioritize", Default = null)]
    public string OverriddenPriorities { get; set; }

    private List<bool>? priorities;

    /// <summary>
    /// A list of overriden priorities from the user. If they want to prioritize the 5th territory, they would input 5.
    /// If they want to prioritize multiple territories, they provide for example "1,2,3,4".
    /// </summary>
    public List<bool>? Priorities
    {
        get
        {
            if (priorities != null)
            {
                return priorities;
            }

            if (!string.IsNullOrWhiteSpace(OverriddenPriorities))
            {
                if (Regex.IsMatch(OverriddenPriorities, @"[\d,]*"))
                {
                    var entries = OverriddenPriorities.Split(',', StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();

                    return priorities ??= Enumerable.Range(1, 18).Select(i => entries.Contains(i.ToString())).ToList();
                }

                throw new Exception("Expected an override list in comma separated format eg. 1,2,3,4");
            }

            return null;
        }
    }
}