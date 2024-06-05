using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Class to store the products from database.
/// </summary>
public class CMSProducts : MonoBehaviour
{
  // Config
  public int productsPerPage = 18;

  // JSON result product interpretation.
  public class CMSProduct
  {
    public class CMSProductImage
    {
      public string src;
    }

    public int id;
    public string sku;
    public string name;
    public string price;
    public CMSProductImage[] images;
  }

  // The list of products to be accessed outside this script.
  public static CMSProduct[] products;
  public static Vector3 pickedProductInitialPosition;
  public static Quaternion pickedProductInitialRotation;

  private void Start()
  {
    StartCoroutine(GetProducts(UpdateProducts));
  }

  /// <summary>
  /// Update products list, called after they are fetched.
  /// </summary>
  /// <param name="_products">Products list result fetched from request.</param>
  void UpdateProducts(CMSProduct[] _products)
  {
    products = _products;
  }

  /// <summary>
  /// Fetch products from database using a UnityWebRequest and API keys from ENV file.
  /// </summary>
  /// <param name="callback">Set method for the products. Used after they're fetched.</param>
  public IEnumerator GetProducts(System.Action<CMSProduct[]> callback)
  {
    while (!EnvReader.isEnvReady)
    {
      yield return null;
    }

    string consumer_key = EnvReader.GetValue("consumer_key");
    string consumer_secret = EnvReader.GetValue("consumer_secret");

    string keys = $"consumer_key={consumer_key}&consumer_secret={consumer_secret}";
    string filters = $"per_page={productsPerPage}";
    string param = $"{keys}&{filters}";

    using UnityWebRequest web = UnityWebRequest.Get(
      "https://cms.fmkey.bmawebdev.ro/wp-json/wc/v3/products?" + param
    );

    yield return web.SendWebRequest();

    if (web.result != UnityWebRequest.Result.Success)
    {
      yield break;
    }

    products = JsonConvert.DeserializeObject<CMSProduct[]>(web.downloadHandler.text);

    callback?.Invoke(products);
  }
}
