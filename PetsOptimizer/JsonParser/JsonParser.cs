namespace PetsOptimizer.JsonParser;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System.Reflection;

public class JsonArrayIndexAttribute : Attribute
{
    public int Index { get; }

    public JsonArrayIndexAttribute(int index)
    {
        Index = index;
    }
}

public class ArrayToObjectConverter<T> : JsonConverter where T : class, new()
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(T);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var array = JArray.Load(reader);

        if (array.First.ToString() == "none")
        {
            return default;
        }

        var propsByIndex = typeof(T).GetProperties()
            .Where(p => p.CanRead && p.CanWrite && p.GetCustomAttribute<JsonArrayIndexAttribute>() != null)
            .ToDictionary(p => p.GetCustomAttribute<JsonArrayIndexAttribute>().Index);

        var obj = new JObject(array
            .Select((jt, i) =>
            {
                PropertyInfo prop;
                return propsByIndex.TryGetValue(i, out prop) ? new JProperty(prop.Name, jt) : null;
            })
            .Where(jp => jp != null)
        );

        var target = new T();
        serializer.Populate(obj.CreateReader(), target);

        return target;
    }

    public override bool CanWrite => false;

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}

