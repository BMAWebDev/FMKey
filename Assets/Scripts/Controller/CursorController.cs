using UnityEngine;

public class CursorController : MonoBehaviour
{
  public Texture2D cursor;
  static Canvas WebGLCursorCanvas;

  public static bool ShowWebGLCursorCanvas
  {
    get => WebGLCursorCanvas.enabled;
    set => WebGLCursorCanvas.enabled = value;
  }

  private void Start()
  {
    WebGLCursorCanvas = transform.Find("WebGLCursorCanvas").GetComponent<Canvas>();

    // How should the cursor look when in the checkout panels
    Cursor.SetCursor(cursor, new Vector2(cursor.width / 2, cursor.height / 2), CursorMode.Auto);
    Cursor.lockState = CursorLockMode.Locked;
  }

  public static void ToggleCursor()
  {
    Cursor.lockState =
      Cursor.lockState == CursorLockMode.None ? CursorLockMode.Locked : CursorLockMode.None;

    if (Cursor.lockState != CursorLockMode.Locked && ShowWebGLCursorCanvas)
    {
      ShowWebGLCursorCanvas = false;
    }
    else if (Cursor.lockState == CursorLockMode.Locked && !ShowWebGLCursorCanvas)
    {
      ShowWebGLCursorCanvas = true;
    }
  }
}
