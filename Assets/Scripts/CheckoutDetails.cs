using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class CheckoutDetails : MonoBehaviour
{
  public static string productTitle;
  public static string productPrice;
  public static int productID;
  public static Sprite productThumbnail;
  public static string lastName;
  public static string firstName;
  public static string address;
  public static string phoneNumber;
  public static string email;
  public static string thumbnailURL;

  public static event Action TextChanged;
  public static event Action ThumbnailChanged;

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

  public static void SetLastName(string _lastName)
  {
    LastName = _lastName;
  }
  public static void SetFirstName(string _firstName)
  {
    FirstName = _firstName;
  }
  public static void SetAddress(string _address)
  {
    Address = _address;
  }
  public static void SetPhoneNumber(string _phoneNumber)
  {
    PhoneNumber = _phoneNumber;
  }
  public static void SetEmail(string _email)
  {
    Email = _email;
  }
  public static void SetProductDetails(int _id, string _title, string _price, string _thumbnailURL)
  {
    productID = _id;
    ProductTitle = _title;
    ProductPrice = _price;
    thumbnailURL = _thumbnailURL;
  }

  public static void LoadImageSprite(MonoBehaviour instance)
  {
    instance.StartCoroutine(LoadImage(thumbnailURL));
  }

  static IEnumerator LoadImage(string imageURL)
  {
    UnityWebRequest web = UnityWebRequestTexture.GetTexture(imageURL);
    yield return web.SendWebRequest();

    if (web.result != UnityWebRequest.Result.Success)
    {
      Debug.Log(web.error);
      yield break;
    }

    Texture2D texture = ((DownloadHandlerTexture)web.downloadHandler).texture;
    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

    ProductThumbnail = sprite;
  }
}
