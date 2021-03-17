using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
public class JsonHelper
{
    public static List<string> InvalidJsonElements;
    public static IList<T> DeserializeToList<T>(string jsonStrData)
    {
        InvalidJsonElements = null;
        var jsonObject = JObject.Parse(jsonStrData);
        var results = jsonObject["annotations"][0]["data"].Children();
        
        IList<T> objectsList = new List<T>();

        foreach (var result in results)
        {
            try
            {
                // CorrectElements  
                objectsList.Add(result.ToObject<T>());
            }
            catch (Exception)
            {
                InvalidJsonElements = InvalidJsonElements ?? new List<string>();
                InvalidJsonElements.Add(result.ToString());
            }
        }

        return objectsList;
    }
}