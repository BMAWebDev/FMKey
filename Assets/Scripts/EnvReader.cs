using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class EnvReader : MonoBehaviour
{
  public static string GetValue(string key)
  {
    string filePath = Application.dataPath.Replace("/Assets", "") + "/.env";

    foreach (string line in File.ReadAllLines(filePath))
    {
      string[] data = line.Split('=');
      if (data.Length == 2 && data[0] == key)
      {
        // Return the value for the given key
        return data[1];
      }
    }

    // Key not found
    return null;
  }
}
