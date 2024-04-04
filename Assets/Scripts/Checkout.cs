using UnityEngine;

public class Checkout : MonoBehaviour
{
  public Canvas checkoutCanvas;
  private int step = 0;
  private readonly int MAX_STEP_INDEX = 4;

  private void Start()
  {
    checkoutCanvas.enabled = false;
    SwitchPanel();
  }

  private void Update()
  {
    if (checkoutCanvas.enabled && Input.GetKeyUp(KeyCode.E) && step == 0)
    {
      step++;
      SwitchPanel();
      HandleCursor();
    }
  }

  public void IncreaseStep()
  {
    step++;

    if (step == MAX_STEP_INDEX)
    {
      // Place order, then handle the rest of the steps
      Order.PlaceOrder(this);

      step = 0;
      checkoutCanvas.enabled = false;
      HandleCursor();
      return;
    }

    SwitchPanel();
  }

  public void DecreaseStep()
  {
    step--;

    if (step == 0)
    {
      checkoutCanvas.enabled = false;
      HandleCursor();
      return;
    }

    SwitchPanel();
  }

  private void SwitchPanel()
  {
    foreach (Transform child in checkoutCanvas.gameObject.transform) { child.gameObject.SetActive(false); }

    string panelName = "";

    switch (step)
    {
      case 0:
        panelName = "Begin";
        break;
      
      case 1:
        panelName = "Product Details";
        break;

      case 2:
        panelName = "Shipping Information";
        break;

      case 3:
        panelName = "Confirm Checkout";
        break;
    }

    GameObject panel = checkoutCanvas.gameObject.transform.Find($"{panelName} Panel").gameObject;
    panel.SetActive(true);
  }

  private void HandleCursor()
  {
    Time.timeScale = Time.timeScale == 0 ? 1 : 0;
    Cursor.lockState = Cursor.lockState == CursorLockMode.None ? CursorLockMode.Locked : CursorLockMode.None;
    Cursor.visible = !Cursor.visible;
    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.CompareTag("player"))
    {
      checkoutCanvas.enabled = true;
      CheckoutDetails.LoadImageSprite(this);
    }
  }

  private void OnTriggerExit(Collider other)
  {
    if (other.gameObject.CompareTag("player"))
    {
      checkoutCanvas.enabled = false;
    }
  }
}
