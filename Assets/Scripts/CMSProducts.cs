using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class CMSProducts : MonoBehaviour
{
  public static JSONNode products;
  public static Vector3 pickedProductInitialPosition;
  public static Quaternion pickedProductInitialRotation;

  private void Start()
  {
    StartCoroutine(GetProducts(UpdateProducts));
  }

  void UpdateProducts(JSONNode _products)
  {
    products = _products;
  }

  public IEnumerator GetProducts(System.Action<JSONNode> callback)
  {
    string consumer_key = EnvReader.GetValue("consumer_key");
    string consumer_secret = EnvReader.GetValue("consumer_secret");

    string param = $"consumer_key={consumer_key}&consumer_secret={consumer_secret}";

    using UnityWebRequest web = UnityWebRequest.Get("https://cms.fmkey.bmawebdev.ro/wp-json/wc/v3/products?" + param);
    yield return web.SendWebRequest();
    
    if(web.result != UnityWebRequest.Result.Success)
    {
      Debug.Log(web.error);
      yield break;
    }

    JSONNode products = JSON.Parse(web.downloadHandler.text);

    callback?.Invoke(products);
  }
}
