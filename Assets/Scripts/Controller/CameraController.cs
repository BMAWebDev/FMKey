using UnityEngine;

public class CameraController : MonoBehaviour
{
  public float sensitivityX;
  public float sensitivityY;

  public Transform orientation;

  private float xRotation;
  private float yRotation;

  public bool canRotate = true;

  private void Update()
  {
    if (canRotate && !Settings.isPaused)
    {
      HandleRotation();
    }
  }

  /// <summary>
  /// Camera controller for rotating via mouse.
  /// </summary>
  private void HandleRotation()
  {
    float mouseX = Input.GetAxisRaw("Mouse X") * sensitivityX;
    float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivityY;

    yRotation += mouseX;

    xRotation -= mouseY;
    xRotation = Mathf.Clamp(xRotation, -90f, 90f);

    transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    orientation.rotation = Quaternion.Euler(0, yRotation, 0);
  }
}
