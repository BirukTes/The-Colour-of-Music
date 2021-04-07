using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class JsonHelper
{
    public static List<string> InvalidJsonElements;
    public static IList<T> DeserializeToList<T>(string jsonStrData)
    {
        IList<T> objectsList = new List<T>();
        try
        {
            InvalidJsonElements = null;
            var jsonObject = JObject.Parse(jsonStrData);
            var results = jsonObject["annotations"][0]["data"].Children();

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
        }
        catch (Exception ex)
        {
            Debug.LogError("error json: " + ex.Message);
        }
        return objectsList;
    }
}