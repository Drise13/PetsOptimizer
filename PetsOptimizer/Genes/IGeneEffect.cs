namespace PetsOptimizer.Genes;

using PetsOptimizer;

public interface IGeneEffect
{
    public enum GeneApplication
    {
        Individual,
        All
    }

    public GeneApplication Application { get; }

    public double StrengthMultiplier { get; }

    public bool DoesMultiplierApplyToForaging(Territory territory);
}

public interface IFighterGeneEffect : IGeneEffect { }

public interface IForagerGeneEffect : IGeneEffect { }