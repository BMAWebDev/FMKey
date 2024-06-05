using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Helper class.
/// <para />
/// Defines properties and methods for helping out with
/// checkout administration.
/// </summary>
public class CheckoutDetails : MonoBehaviour
{
  // Config
  public const int phoneNumberMinCharCount = 9;
  public const int textMinCharCount = 0;

  // UI text fields to be displayed
  public static string productTitle;
  public static string productPrice;
  public static int productID = 0;
  public static Sprite productThumbnail;
  public static string thumbnailURL;

  // UI input fields that can be modified
  public static string lastName = "";
  public static string firstName = "";
  public static string address = "";
  public static string phoneNumber = "";
  public static string email = "";

  /*
    Error properties
   
    These properties are initially set in the TextUpdater script
    They are used for displaying an error after validating input fields.
  */
  public static GameObject emailError;
  public static GameObject phoneError;
  public static GameObject addressError;
  public static GameObject lastNameError;
  public static GameObject firstNameError;

  /*
    Error helpers
   
    These are used to store the error references. Beside having just the UI,
    we also need to have control over them (for submitting or when values change).
  */
  class CurrentFieldParam
  {
    public string field;
    public Error error;
  }

  public enum Error
  {
    Address,
    LastName,
    FirstName,
    PhoneNumber,
    Email,
  };

  public static List<Error> errors = new();

  /*
    Event Listeners
   
    They are triggered everytime a value changes,
    (for example text or thumbnail)
  */
  public static event Action TextChanged;
  public static event Action ThumbnailChanged;

  /*
    Variable modifiers
   
    They get and set variables, triggering events when doing so.
  */
  public static string ProductTitle
  {
    get => productTitle;
    set
    {
      productTitle = value;
      TextChanged.Invoke();
    }
  }
  public static string ProductPrice
  {
    get => productPrice;
    set
    {
      productPrice = value;
      TextChanged.Invoke();
    }
  }
  public static Sprite ProductThumbnail
  {
    get => productThumbnail;
    set
    {
      productThumbnail = value;
      ThumbnailChanged.Invoke();
    }
  }
  public static string LastName
  {
    get => lastName;
    set
    {
      lastName = value;
      TextChanged.Invoke();
    }
  }
  public static string FirstName
  {
    get => firstName;
    set
    {
      firstName = value;
      TextChanged.Invoke();
    }
  }
  public static string Address
  {
    get => address;
    set
    {
      address = value;
      TextChanged.Invoke();
    }
  }
  public static string PhoneNumber
  {
    get => phoneNumber;
    set
    {
      phoneNumber = value;
      TextChanged.Invoke();
    }
  }
  public static string Email
  {
    get => email;
    set
    {
      email = value;
      TextChanged.Invoke();
    }
  }

  /// <summary>
  /// Set product's details.
  /// <br />
  /// Called in the Pickup script, it is triggered
  /// when player selects a new product.
  /// </summary>
  /// <param name="_id">Product's ID. Used for order placement.</param>
  /// <param name="_title">Product's title. Used for display.</param>
  /// <param name="_price">Product's price. Used for display.</param>
  /// <param name="_thumbnailURL">Product's thumbnail URL. Used for display.</param>
  public static void SetProductDetails(int _id, string _title, string _price, string _thumbnailURL)
  {
    productID = _id;
    ProductTitle = _title;
    ProductPrice = _price;
    thumbnailURL = _thumbnailURL;
  }

  /// <summary>
  /// Helper function. Calls LoadImage based on the product's thumbnail URL.
  /// Needs a MonoBehaviour instance.
  /// <br />
  /// Called in the Checkout script, it is triggered
  /// when player collides with the checkout cash desk,
  /// in order to prevent
  /// </summary>
  /// <param name="instance">MonoBehaviour instance that handles the image call.</param>
  public static void LoadImageSprite(MonoBehaviour instance)
  {
    if (thumbnailURL != null)
    {
      instance.StartCoroutine(LoadImage(thumbnailURL));
    }
  }

  /// <summary>
  /// Sends a UnityWebRequest to get (and set) the product's thumbnail based on the URL saved.
  /// </summary>
  /// <param name="imageURL">Product's thumbnail URL.</param>
  static IEnumerator LoadImage(string imageURL)
  {
    // Product images are accessible by URL, so no need for API Keys here
    UnityWebRequest web = UnityWebRequestTexture.GetTexture(imageURL);
    yield return web.SendWebRequest();

    if (web.result != UnityWebRequest.Result.Success)
    {
      yield break;
    }

    // Get the web result as a Texture and create a Sprite out of it
    Texture2D texture = ((DownloadHandlerTexture)web.downloadHandler).texture;
    Sprite sprite = Sprite.Create(
      texture,
      new Rect(0, 0, texture.width, texture.height),
      new Vector2(0.5f, 0.5f)
    );

    // Update product's thumbnail
    ProductThumbnail = sprite;
  }

  /// <summary>
  /// Submit the shipping information panel details.
  /// Called on click on the continue button,
  /// checks for issues before submitting.
  /// </summary>
  public static void SubmitShippingInfo()
  {
    // Create fields
    CurrentFieldParam[] currentFieldParams =
    {
      new() { field = Email, error = Error.Email },
      new() { field = PhoneNumber, error = Error.PhoneNumber },
      new() { field = Address, error = Error.Address },
      new() { field = LastName, error = Error.LastName },
      new() { field = FirstName, error = Error.FirstName },
    };

    // Check for errors
    CheckCurrentFieldsErrors(currentFieldParams);

    // If there are errors listed, handle them
    if (errors.Count > 0)
    {
      HandleErrors();
      return;
    }

    Checkout checkoutScript = GameObject.Find("Checkout Collider").GetComponent<Checkout>();
    checkoutScript.IncreaseStep();
  }

  // Set fields input for the Shipping Information Panel. Triggered on value changed.
  public static void SetLastName(string lastName)
  {
    bool isFieldValid = GetIsFieldValidAndHandleErrors(lastName, Error.LastName);

    if (isFieldValid)
    {
      LastName = lastName;
    }
  }

  public static void SetFirstName(string firstName)
  {
    bool isFieldValid = GetIsFieldValidAndHandleErrors(firstName, Error.FirstName);

    if (isFieldValid)
    {
      FirstName = firstName;
    }
  }

  public static void SetAddress(string address)
  {
    bool isFieldValid = GetIsFieldValidAndHandleErrors(address, Error.Address);

    if (isFieldValid)
    {
      Address = address;
    }
  }

  public static void SetPhoneNumber(string phoneNumber)
  {
    bool isFieldValid = GetIsFieldValidAndHandleErrors(phoneNumber, Error.PhoneNumber);

    if (isFieldValid)
    {
      PhoneNumber = phoneNumber;
    }
  }

  public static void SetEmail(string email)
  {
    bool isFieldValid = GetIsFieldValidAndHandleErrors(email, Error.Email);

    if (isFieldValid)
    {
      Email = email;
    }
  }

  /// <summary>
  /// Check whether the given field has a valid value.
  /// </summary>
  /// <param name="_field">Field to be checked.</param>
  /// <param name="fieldError">The type of error that is checked.</param>
  /// <returns>Boolean to see if value is valid.</returns>
  static bool GetIsFieldValidAndHandleErrors(string _field, Error fieldError)
  {
    bool isFieldValid = fieldError switch
    {
      Error.Email => Functions.IsEmailValid(_field),
      _
        => Functions.IsTextValid(
          _field,
          fieldError == Error.PhoneNumber ? phoneNumberMinCharCount : textMinCharCount
        ),
    };

    if (isFieldValid && errors.Contains(fieldError))
    {
      errors.Remove(fieldError);
    }
    else if (!isFieldValid && !errors.Contains(fieldError))
    {
      errors.Add(fieldError);
    }

    HandleErrors();

    return isFieldValid;
  }

  /// <summary>
  /// Check in the given fields if there is any error and if there is,
  /// add it to the list of available errors.
  /// </summary>
  /// <param name="fieldList">Fields to be checked.</param>
  static void CheckCurrentFieldsErrors(CurrentFieldParam[] fieldList)
  {
    foreach (CurrentFieldParam currentField in fieldList)
    {
      if (
        currentField.error == Error.Email && !Functions.IsEmailValid(currentField.field)
        || !Functions.IsTextValid(
          currentField.field,
          currentField.error == Error.PhoneNumber ? phoneNumberMinCharCount : textMinCharCount
        )
      )
      {
        errors.Add(currentField.error);
      }
    }
  }

  /// <summary>
  /// Loops through each error available in the Error Enum and
  /// activates in UI the text for the available errors.
  /// </summary>
  public static void HandleErrors()
  {
    // Loop through the Error Enum and get the values
    foreach (Error error in Enum.GetValues(typeof(Error)))
    {
      // Set it to be activated/deactivated in UI based on its presence in the errors list
      SetErrorTextActiveFromError(error, errors.Contains(error));
    }
  }

  /// <summary>
  /// Set error's UI text visibility.
  /// </summary>
  /// <param name="error">The error to have the visibility changed.</param>
  /// <param name="shouldBeActive">The visibility status.</param>
  static void SetErrorTextActiveFromError(Error error, bool shouldBeActive)
  {
    switch (error)
    {
      case Error.Email:
        emailError.SetActive(shouldBeActive);
        break;

      case Error.PhoneNumber:
        phoneError.SetActive(shouldBeActive);
        break;

      case Error.Address:
        addressError.SetActive(shouldBeActive);
        break;

      case Error.LastName:
        lastNameError.SetActive(shouldBeActive);
        break;

      case Error.FirstName:
        firstNameError.SetActive(shouldBeActive);
        break;
    }
  }
}
