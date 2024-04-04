using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
  public Texture2D cursor;

  // Start is called before the first frame update
  void Awake()
  {
    Cursor.SetCursor(cursor, new Vector2(cursor.width / 2, cursor.height / 2), CursorMode.ForceSoftware);
    Cursor.lockState = CursorLockMode.Locked;
  }

  private void Start()
  {
    // https://docs.unity3d.com/ScriptReference/Application-targetFrameRate.html
    // Make the app run as fast as possible
    Application.targetFrameRate = -1;
  }

  private void Update()
  {
    Cursor.visible = true;
  }

}
