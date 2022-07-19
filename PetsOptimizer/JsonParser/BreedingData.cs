namespace PetsOptimizer.JsonParser;

using Genes;

using MoreLinq.Extensions;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

public class BreedingData
{
    private double? fightContribution;

    private List<Pet>? pets;

    private int? territories;

    [JsonProperty("Pets")] public List<PetData?> PetData { private get; set; }

    [JsonProperty("PetsStored")] public List<PetData?> PetsStored { private get; set; }

    [JsonProperty("Territory")] public JArray Territory { private get; set; }
    [JsonProperty("Breeding")] public JArray Breeding { private get; set; }

    [JsonIgnore] public int Territories => territories ??= Territory.Count(i => i.First.ToObject<int>() != 0);

    [JsonIgnore] public double FightContribution => fightContribution ??= Breeding[2][6].ToObject<int>() * 0.06;

    [JsonIgnore]
    public IEnumerable<Pet> Pets =>
        pets ??= PetData.Concat(PetsStored)
            .Where(p => p is
            {
                //Excluding foragers that typically reside in the fence yard
                Genetics: not PetGenetics.Breeder or PetGenetics.Special or PetGenetics.Eggshell or PetGenetics.Trasher
            })
            .Select(petData => new Pet(petData))
            .ToList();

    [JsonIgnore]
    public int PetCount => pets?.Count ?? 0;

    [JsonIgnore] public IEnumerable<Pet> ShuffledPets => Pets.Shuffle();

    [JsonIgnore] public List<bool>? Overrides { get; set; }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented, new StringEnumConverter());
    }
}