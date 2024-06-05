using System;
using System.Threading;
using TMPro;
using UnityEngine;

public class Checkout : MonoBehaviour
{
  public Canvas checkoutCanvas;
  static readonly int MIN_STEP_INDEX = 0;
  public static int step = MIN_STEP_INDEX;
  readonly int MAX_STEP_INDEX = 4;
  public GameObject loadingText;
  public GameObject failText;
  public GameObject successText;

  enum CheckoutPanel
  {
    Begin,
    ProductDetails,
    ShippingInformation,
    ConfirmCheckout,
    OrderStatus
  }

  private void Start()
  {
    checkoutCanvas.enabled = false;
    SwitchPanel();
  }

  private void Update()
  {
    if (checkoutCanvas.enabled && Input.GetKeyUp(KeyCode.E) && step == MIN_STEP_INDEX)
    {
      step++;
      SwitchPanel();
      HandlePause();
    }
  }

  public void IncreaseStep()
  {
    step++;
    SwitchPanel();

    if (step == MAX_STEP_INDEX)
    {
      // Place order, then handle the rest of the steps
      Order.PlaceOrder(this, HandleOrderStatusPanel);
    }
  }

  void DisableOrderStatusMessageAndReset()
  {
    // disable status texts, but keep the loading one
    failText.SetActive(false);
    successText.SetActive(false);
    loadingText.SetActive(true);

    // reset to first step in order to enable a second purchase
    step = MIN_STEP_INDEX;
    SwitchPanel();
  }

  public void HandleOrderStatusPanel(int orderID = 0)
  {
    loadingText.SetActive(false);

    // order has failed
    if (orderID == 0)
    {
      failText.SetActive(true);
    }
    else
    {
      successText.SetActive(true);
      successText.GetComponent<TextMeshProUGUI>().text =
        $"Your order number #{orderID} has been placed";
    }

    // re-enable the cursor
    HandlePause();

    // show the status text for 5 seconds,
    // then disable checkout panels
    Invoke(nameof(DisableOrderStatusMessageAndReset), 5);
  }

  public void DecreaseStep()
  {
    step--;
    SwitchPanel();

    if (step == MIN_STEP_INDEX)
    {
      HandlePause();
    }
  }

  private void SwitchPanel()
  {
    foreach (Transform child in checkoutCanvas.transform)
    {
      child.gameObject.SetActive(false);
    }

    string panelName = Enum.GetName(typeof(CheckoutPanel), step);

    GameObject panel = checkoutCanvas.gameObject.transform.Find($"{panelName} Panel").gameObject;
    panel.SetActive(true);
  }

  private void HandlePause()
  {
    Settings.isPaused = !Settings.isPaused;
    CursorController.ToggleCursor();
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.CompareTag("player") && CheckoutDetails.productID != 0)
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
