using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Class to read from the Environment file.
/// </summary>
public class EnvReader : MonoBehaviour
{
  static string[] envFileLines;
  public static bool isEnvReady = false;

  private void Awake()
  {
    StartCoroutine(GetEnvFile());
  }

  /// <summary>
  /// Fetch the env file.
  /// </summary>
  IEnumerator GetEnvFile()
  {
    string filePath;

    if (Application.platform == RuntimePlatform.WebGLPlayer)
    {
      // Root path on the server
      filePath = Application.absoluteURL + ".env";
    }
    else
    {
      // Root path in the Editor
      filePath = Application.dataPath.Replace("Assets", "") + ".env";

      // Prefix location path with a file:// if running on MacOS.
      // Windows doesn't need it.
#if UNITY_EDITOR_OSX
      filePath = "file://" + filePath;
#endif
    }

    // Load the file using UnityWebRequest
    UnityWebRequest www = UnityWebRequest.Get(filePath);
    yield return www.SendWebRequest();

    if (www.result != UnityWebRequest.Result.Success)
    {
      yield break;
    }

    envFileLines = www.downloadHandler.text.Split('\n');
    isEnvReady = true;

    yield break;
  }

  /// <summary>
  /// Get a value from the env file based on its key.
  /// </summary>
  /// <param name="key">Key to fetch the value from</param>
  /// <returns></returns>
  public static string GetValue(string key)
  {
    // Check and wait for ENV File to be ready to be read from
    while (!isEnvReady)
    {
      return null;
    }

    foreach (string line in envFileLines)
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
