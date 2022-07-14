// ReSharper disable IdentifierTypo
namespace PetsOptimizer.Genes;

using System.Diagnostics;

using static IGeneEffect.GeneApplication;

public class NoEffect : IGeneEffect
{
    public NoEffect(Pet pet, PetGenetics petGenetic)
    {
        Console.WriteLine($"Warning: Pet with no effect created {pet.Species} {petGenetic}");

        if (!Enum.IsDefined(typeof(PetGenetics), petGenetic) && Debugger.IsAttached)
        {
            throw new Exception($"Did not parse genetic value {petGenetic} for pet {pet.Species}");
        }
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

//TODO implement above/below fight power modification
public class TsarEffect : IFighterGeneEffect
{
    public IGeneEffect.GeneApplication Application => All;
    public double StrengthMultiplier => 1.5;

    public bool DoesMultiplierApplyToForaging(Territory territory)
    {
        return false;
    }
}