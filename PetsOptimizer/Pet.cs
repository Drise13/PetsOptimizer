namespace PetsOptimizer;

using Genes;

using JsonParser;

public class Pet
{
    private readonly PetGenetics genetics;

    public Pet(PetData data)
    {
        Species = data.Species;
        genetics = data.Genetics;
        GeneEffect = GeneticFactory.GetGeneticEffect(this, genetics);
        Strength = data.Strength;
    }

    public Species Species { get; }

    public IGeneEffect GeneEffect { get; }

    public double Strength { get; }

    public override string ToString()
    {
        return $"{Species} {genetics} {Math.Floor(Strength)}";
    }
}