namespace PetsOptimizer.Genes;

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