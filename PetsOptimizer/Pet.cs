namespace PetsOptimizer;

public class Pet
{
    public Pet(PetData data)
    {
        Species = data.Species;
        GeneEffect = GeneticFactory.GetGeneticEffect(this, data.Genetics);
        Strength = data.Strength;
    }

    public Species Species { get; }

    public IGeneEffect GeneEffect { get; }

    public double Strength { get; }
}