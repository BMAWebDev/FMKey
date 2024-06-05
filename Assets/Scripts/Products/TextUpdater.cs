using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextUpdater : MonoBehaviour
{
  public TextMeshProUGUI productHoverNameValue;
  public TextMeshProUGUI productDetailsNameValue;
  public TextMeshProUGUI confirmOrderNameValue;

  public TextMeshProUGUI productHoverPriceValue;
  public TextMeshProUGUI productDetailsPriceValue;
  public TextMeshProUGUI confirmOrderPriceValue;

  public TextMeshProUGUI confirmOrderFullNameValue;
  public TextMeshProUGUI confirmOrderAddressValue;
  public TextMeshProUGUI confirmOrderPhoneNumberValue;
  public TextMeshProUGUI confirmOrderEmailValue;

  public Image productDetailsThumbnail;
  public Image confirmOrderThumbnail;

  // Shipping Information UI objects
  public GameObject addressError;
  public GameObject lastNameError;
  public GameObject firstNameError;
  public GameObject emailError;
  public GameObject phoneError;

  private void Start()
  {
    // Subscribe to property change events
    CheckoutDetails.TextChanged += UpdateProductDetails;
    CheckoutDetails.ThumbnailChanged += UpdateThumbnail;

    // Initialize text
    UpdateProductDetails();
    UpdateThumbnail();

    // Set the UI references of shipping information panel
    CheckoutDetails.addressError = addressError;
    CheckoutDetails.lastNameError = lastNameError;
    CheckoutDetails.firstNameError = firstNameError;
    CheckoutDetails.emailError = emailError;
    CheckoutDetails.phoneError = phoneError;
  }

  void UpdateProductDetails()
  {
    // Update TMP components with static properties
    productHoverNameValue.text = CheckoutDetails.ProductTitle;
    productDetailsNameValue.text = CheckoutDetails.ProductTitle;
    confirmOrderNameValue.text = CheckoutDetails.ProductTitle;

    productHoverPriceValue.text = $"{CheckoutDetails.ProductPrice} RON";
    productDetailsPriceValue.text = $"{CheckoutDetails.ProductPrice} RON";
    confirmOrderPriceValue.text = $"{CheckoutDetails.ProductPrice} RON";

    confirmOrderFullNameValue.text = $"{CheckoutDetails.LastName} {CheckoutDetails.FirstName}";
    confirmOrderAddressValue.text = CheckoutDetails.Address;
    confirmOrderPhoneNumberValue.text = CheckoutDetails.PhoneNumber;
    confirmOrderEmailValue.text = CheckoutDetails.Email;
  }

  void UpdateThumbnail()
  {
    productDetailsThumbnail.sprite = CheckoutDetails.ProductThumbnail;
    confirmOrderThumbnail.sprite = CheckoutDetails.ProductThumbnail;
  }

  private void OnDestroy()
  {
    // Unsubscribe from property change events
    CheckoutDetails.TextChanged -= UpdateProductDetails;
    CheckoutDetails.ThumbnailChanged -= UpdateProductDetails;
  }
}
