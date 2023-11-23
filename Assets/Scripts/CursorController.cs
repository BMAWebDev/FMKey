using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
  public Texture2D cursor;

  // Start is called before the first frame update
  void Awake()
  {
    //Cursor.SetCursor(cursor, new Vector2(cursor.width / 2, cursor.height / 2), CursorMode.Auto);
    Cursor.lockState = CursorLockMode.Locked;
  }

  private void Update()
  {
    Cursor.visible = true;
  }

}
