using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// General settings file.
/// </summary>
public class Settings : MonoBehaviour
{
  public static bool isPaused = false;

  private void Start()
  {
    // https://docs.unity3d.com/ScriptReference/Application-targetFrameRate.html
    // Make the app run as fast as possible
    Application.targetFrameRate = -1;
  }
}
