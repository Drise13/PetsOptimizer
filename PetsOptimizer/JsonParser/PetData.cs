namespace PetsOptimizer.JsonParser;

using JsonParser;

using Newtonsoft.Json;
using PetsOptimizer;
using PetsOptimizer.Genes;

[JsonConverter(typeof(ArrayToObjectConverter<PetData>))]
public class PetData
{
    [JsonArrayIndex(0)] public Species Species { get; set; }

    [JsonArrayIndex(1)] public PetGenetics Genetics { get; set; }

    [JsonArrayIndex(2)] public double Strength { get; set; }

    [JsonArrayIndex(3)] public double Unknown { get; set; }
}