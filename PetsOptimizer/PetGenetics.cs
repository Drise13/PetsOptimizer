namespace PetsOptimizer;

using static IGeneEffect.GeneApplication;

// sort using http://textmechanic.com/text-tools/basic-text-tools/sort-text-lines/
// ReSharper disable IdentifierTypo
public enum PetGenetics
{
    Defender = 1,
    Forager = 2,
    Fleeter = 3,
    Breeder = 4,
    Mercenary = 6,
    Sniper = 8,
    Amplifier = 9,
    Cursory = 12,
    Fastidious = 13,
    Opticular = 15,
    Defstone = 19,
    Refiller = 22
}

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

public static class GeneticFactory
{
    public static IGeneEffect GetGeneticEffect(Pet pet, PetGenetics petGenetic)
    {
        return petGenetic switch
        {
            PetGenetics.Forager => new ForagerEffect(),
            PetGenetics.Fleeter => new FleeterEffect(),
            PetGenetics.Mercenary => new MercenaryEffect(),
            PetGenetics.Fastidious => new FastidiousEffect(),
            PetGenetics.Opticular => new OpticularEffect(pet),
            _ => new NoEffect(pet)
        };
    }
}

public class NoEffect : IGeneEffect
{
    public NoEffect(Pet pet)
    {
        Console.WriteLine($"Warning: Pet with no effect created {pet.Species}");
    }

    public IGeneEffect.GeneApplication Application => Individual;

    public double StrengthMultiplier => 1.0;

    public bool DoesMultiplierApplyToForaging(Territory territory)
    {
        return false;
    }
}

public class ForagerEffect : IForagerGeneEffect
{
    public IGeneEffect.GeneApplication Application => Individual;
    public double StrengthMultiplier => 2.0;

    public bool DoesMultiplierApplyToForaging(Territory territory)
    {
        return true;
    }
}

public class FleeterEffect : IForagerGeneEffect
{
    public IGeneEffect.GeneApplication Application => All;
    public double StrengthMultiplier => 1.3;

    public bool DoesMultiplierApplyToForaging(Territory territory)
    {
        return true;
    }
}

public class MercenaryEffect : IFighterGeneEffect
{
    public IGeneEffect.GeneApplication Application => Individual;
    public double StrengthMultiplier => 2.0;

    public bool DoesMultiplierApplyToForaging(Territory territory)
    {
        return false;
    }
}

public class FastidiousEffect : IForagerGeneEffect
{
    public IGeneEffect.GeneApplication Application => All;
    public double StrengthMultiplier => 1.5;

    public bool DoesMultiplierApplyToForaging(Territory territory)
    {
        return territory.Pets.Any(p => p.GeneEffect is IFighterGeneEffect);
    }
}

public class OpticularEffect : IForagerGeneEffect
{
    private readonly Pet pet;

    public OpticularEffect(Pet pet)
    {
        this.pet = pet;
    }

    public IGeneEffect.GeneApplication Application => Individual;
    public double StrengthMultiplier => 3.0;

    public bool DoesMultiplierApplyToForaging(Territory territory)
    {
        return territory.Pets.Where(p => p != pet).All(p => pet.Strength > p.Strength);
    }
}