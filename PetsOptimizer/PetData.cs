namespace PetsOptimizer;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

internal class PetData
{
    public List<Pet> Pets { get; set; }
    public int Territories { get; set; }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented, new StringEnumConverter());
    }
}