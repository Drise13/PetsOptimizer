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
            _ => new NoEffect(pet, petGenetic)
        };
    }
}