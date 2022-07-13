namespace PetsOptimizer.JsonParser;

using MoreLinq.Extensions;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using Genes;

public class BreedingData
{
    public List<PetData> PetData { get; set; }
    public int Territories { get; set; }
    public double FightContribution { get; set; }

    private List<Pet>? pets;

    public IEnumerable<Pet> Pets =>
        pets ??= PetData.Where(p => p.Genetics != PetGenetics.Breeder).Select(petData => new Pet(petData))
            .ToList();

    public IEnumerable<Pet> ShuffledPets => Pets.Shuffle();

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented, new StringEnumConverter());
    }
}