namespace PetsOptimizer.Genes;

using System.Diagnostics;

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
            PetGenetics.Tsar => new TsarEffect(),

            PetGenetics.Fighter or
                PetGenetics.Defender or
                PetGenetics.Boomer or
                PetGenetics.Sniper or
                PetGenetics.Amplifier or
                PetGenetics.Rattler or
                PetGenetics.Cursory or
                PetGenetics.Defstone or
                PetGenetics.Lazarus or
                PetGenetics.Heavyweight or
                PetGenetics.Fastihoop or
                PetGenetics.Ninja or
                PetGenetics.Superboomer or
                PetGenetics.Peapeapod => new NoEffectFighter(pet, petGenetic),

            PetGenetics.Special or
                PetGenetics.Breeder or
                PetGenetics.Looter or
                PetGenetics.Refiller or
                PetGenetics.Eggshell or
                PetGenetics.Trasher or
                PetGenetics.Converter or
                PetGenetics.Alchemic or
                PetGenetics.Monolithic => new NoEffectForager(pet, petGenetic),
            _ => new NoEffect(pet, petGenetic)
        };
    }
}