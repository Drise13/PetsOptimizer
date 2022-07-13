using Newtonsoft.Json;

using PetsOptimizer;

var jsonDataString = File.ReadAllText("C:\\Users\\Drise\\cleanBreedingData.json");

var jsonData = JsonConvert.DeserializeObject<BreedingData>(jsonDataString);

Console.Write(jsonData.ToString());