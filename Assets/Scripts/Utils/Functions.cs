using System;
using System.Text.RegularExpressions;
using UnityEngine;

public class Functions : MonoBehaviour
{
  public static bool IsEmailValid(string email)
  {
    // Source: https://haacked.com/archive/2007/08/21/i-knew-how-to-validate-an-email-address-until-i.aspx/
    string emailPattern =
      @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
      + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
      + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

    return new Regex(emailPattern).IsMatch(email.Trim());
  }

  public static bool IsTextValid(string text, int charCount = 0)
  {
    return text.Length > charCount;
  }

  public static double GetProductDiagonalLength(GameObject product)
  {
    Vector3 size = product.GetComponent<BoxCollider>().bounds.size;
    float lat = size.z;
    float lng = size.x;

    // Pythagorean theorem
    // The diagonal length is the hypotenuse
    return Math.Sqrt(Math.Pow(lat, 2) + Math.Pow(lng, 2));
  }

  public static void SetLayerByName(Transform _transform, string layerName)
  {
    int layer = LayerMask.NameToLayer(layerName);

    // Set layer for parent
    _transform.gameObject.layer = layer;

    Transform[] children = _transform.GetComponentsInChildren<Transform>(includeInactive: true);

    // Set layer for all of its children
    foreach (Transform child in children)
    {
      child.gameObject.layer = layer;
    }
  }

  /// <summary>
  /// Check wheter the app is focused or not in order to render the pause menu.
  /// </summary>
  /// <returns>A bool that tells the status of app focus.</returns>
  public static bool GetIsAppUnfocused()
  {
    // Vector3 mousePos = Input.mousePosition;
    Vector3 mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);

    return !Application.isFocused
      || mousePos.x < 0
      || mousePos.x > 1
      || mousePos.y < 0
      || mousePos.y > 1;
  }
}
