using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Order : MonoBehaviour
{
  [System.Serializable]
  class LineItem
  {
    public int product_id;
    public int quantity;
  }

  [System.Serializable]
  class IAddress
  {
    public string first_name;
    public string last_name;
    public string address_1;
    public string address_2 = "";
    public string city = "Bucuresti";
    public string state = "B";
    public string postcode = "041028";
    public string country = "RO";
  }

  [System.Serializable]
  class IBilling : IAddress
  {
    public string phone;
    public string email;
  }

  class IOrder
  {
    public string payment_method = "cod";
    public string payment_method_title = "Cash on delivery";
    public bool set_paid = false;
    public IBilling billing;
    public IAddress shipping;
    public LineItem[] line_items;
  }

  public static void PlaceOrder(MonoBehaviour instance)
  {
    IAddress shipping = new()
    {
      first_name = CheckoutDetails.firstName,
      last_name = CheckoutDetails.lastName,
      address_1 = CheckoutDetails.address
    };

    IBilling billing = new()
    {
      first_name = shipping.first_name,
      last_name = shipping.last_name,
      address_1 = shipping.address_1,
      phone = CheckoutDetails.phoneNumber,
      email = CheckoutDetails.email,
    };

    LineItem[] line_items = new LineItem[] {
      new() { product_id = CheckoutDetails.productID, quantity = 1 },
    };

    IOrder order = new()
    {
      billing = billing,
      shipping = shipping,
      line_items = line_items,
    };

    string jsonData = JsonUtility.ToJson(order);

    Debug.Log(jsonData);

    instance.StartCoroutine(CreateOrder(jsonData));
  }

  static IEnumerator CreateOrder(string jsonData)
  {
    string consumer_key = EnvReader.GetValue("consumer_key");
    string consumer_secret = EnvReader.GetValue("consumer_secret");

    string param = $"consumer_key={consumer_key}&consumer_secret={consumer_secret}";

    using UnityWebRequest web = UnityWebRequest.Post("https://cms.fmkey.bmawebdev.ro/wp-json/wc/v3/orders?" + param, "POST");

    byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
    web.uploadHandler = new UploadHandlerRaw(bodyRaw);
    web.downloadHandler = new DownloadHandlerBuffer();
    web.SetRequestHeader("Content-Type", "application/json");

    yield return web.SendWebRequest();

    if (web.result != UnityWebRequest.Result.Success)
    {
      Debug.Log(web.error);
      yield break;
    }
    else
    {
      string order = web.downloadHandler.text;

      Debug.Log(order);
    }
  }
}
