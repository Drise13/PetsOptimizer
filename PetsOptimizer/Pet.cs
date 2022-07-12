namespace PetsOptimizer;

using System.Text;

using JsonParser;

using Newtonsoft.Json;



[JsonConverter(typeof(ArrayToObjectConverter<Pet>))]
public class Pet
{
    [JsonArrayIndex(0)] public Species Species { get; set; }

    [JsonArrayIndex(1)] public PetGenetics Genetics { get; set; }

    [JsonArrayIndex(2)] public double Strength { get; set; }

    [JsonArrayIndex(3)] public double Unknown { get; set; }

    public bool IsFighter => Genetics != PetGenetics.Breeder;
}