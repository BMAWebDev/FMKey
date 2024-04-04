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

  private void Start()
  {
    // Subscribe to property change events
    CheckoutDetails.TextChanged += UpdateTMPText;
    CheckoutDetails.ThumbnailChanged += UpdateThumbnail;

    // Initialize text
    UpdateTMPText();
    UpdateThumbnail();
  }

  void UpdateTMPText()
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
    CheckoutDetails.TextChanged -= UpdateTMPText;
    CheckoutDetails.ThumbnailChanged -= UpdateTMPText;
  }
}
