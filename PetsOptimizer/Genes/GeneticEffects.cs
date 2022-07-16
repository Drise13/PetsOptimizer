// ReSharper disable IdentifierTypo
namespace PetsOptimizer.Genes;

using System.Diagnostics;

using static IGeneEffect.GeneApplication;

public class NoEffect : IGeneEffect
{
    public NoEffect(Pet pet, PetGenetics petGenetic)
    {
        Console.WriteLine($"Warning: Pet with no effect created {pet.Species} {petGenetic}");

        if (Debugger.IsAttached && !Enum.IsDefined(typeof(PetGenetics), petGenetic))
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

public class NoEffectFighter : NoEffect, IFighterGeneEffect
{
    public NoEffectFighter(Pet pet, PetGenetics petGenetic)
        : base(pet, petGenetic) { }
}

public class NoEffectForager : NoEffect, IForagerGeneEffect
{
    public NoEffectForager(Pet pet, PetGenetics petGenetic)
        : base(pet, petGenetic) { }
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

public class FlashyEffect : IForagerGeneEffect
{
    public IGeneEffect.GeneApplication Application => All;
    public double StrengthMultiplier => 1.5;

    public bool DoesMultiplierApplyToForaging(Territory territory)
    {
        return territory.Pets.All(p => p.GeneEffect is not IFighterGeneEffect);
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

public class TsarEffect : IFighterGeneEffect
{
    public IGeneEffect.GeneApplication Application => Regional;
    public double StrengthMultiplier => 1.5;

    public bool DoesMultiplierApplyToForaging(Territory territory)
    {
        return false;
    }
}

public class BadumdumEffect : IForagerGeneEffect
{
    public IGeneEffect.GeneApplication Application => Regional;
    public double StrengthMultiplier => 1.5;

    public bool DoesMultiplierApplyToForaging(Territory territory)
    {
        return false;
    }
}

public class TargeterEffect : IForagerGeneEffect
{
    private readonly Pet pet;

    public TargeterEffect(Pet pet)
    {
        this.pet = pet;
    }

    public IGeneEffect.GeneApplication Application => Individual;
    public double StrengthMultiplier => 5.0;

    public bool DoesMultiplierApplyToForaging(Territory territory)
    {
        if (territory.TerritoryPosition == 0)
        {
            return false;
        }

        var aboveTerritory = territory.Population.Territories[territory.TerritoryPosition - 1];

        var currentPetIndex = territory.Pets.IndexOf(pet);

        if (currentPetIndex < aboveTerritory.Pets.Count)
        {
            return aboveTerritory.Pets[currentPetIndex].GeneEffect is TargeterEffect;
        }

        return false;
    }
}

public class MiasmaEffect : IForagerGeneEffect
{
    private readonly Pet pet;

    public MiasmaEffect(Pet pet)
    {
        this.pet = pet;
    }

    public IGeneEffect.GeneApplication Application => All;
    public double StrengthMultiplier => 4.0;

    public bool DoesMultiplierApplyToForaging(Territory territory)
    {
        return territory.Pets.Select(p => p.GeneEffect).Distinct().Count() == territory.Pets.Count;
    }
}

public class BorgerEffect : IForagerGeneEffect
{
    private readonly Pet pet;

    public BorgerEffect(Pet pet)
    {
        this.pet = pet;
    }

    public IGeneEffect.GeneApplication Application => Individual;
    public double StrengthMultiplier => 10.0;

    public bool DoesMultiplierApplyToForaging(Territory territory)
    {
        if (territory.TerritoryPosition == 0)
        {
            return false;
        }

        var aboveTerritory = territory.Population.Territories[territory.TerritoryPosition - 1];

        return aboveTerritory.Pets.Any(p => p.GeneEffect is ForagerEffect);
    }
}