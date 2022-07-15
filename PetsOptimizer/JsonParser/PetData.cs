namespace PetsOptimizer.JsonParser;

using Newtonsoft.Json;

using PetsOptimizer;

using Genes;

[JsonConverter(typeof(ArrayToObjectConverter<PetData>))]
public class PetData
{
    [JsonArrayIndex(0)] public Species Species { get; set; }

    [JsonArrayIndex(1)] public PetGenetics Genetics { get; set; }

    [JsonArrayIndex(2)] public double Strength { get; set; }

    [JsonArrayIndex(3)] public double Special { get; set; }
}