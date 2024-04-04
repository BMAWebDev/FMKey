using SimpleJSON;
using UnityEngine;

public class CameraController : MonoBehaviour
{
  public float sensitivityX;
  public float sensitivityY;

  public Transform orientation;

  public GameObject productsScript;

  public Canvas hoverCanvas;
  private GameObject hoverPanel;

  private float xRotation;
  private float yRotation;

  private Renderer rayRenderer = null;
  private bool isProductPickedUp = false;

  private bool isHoveringOverShelf = false;

  private bool IsHoverPanelActive
  {
    get => hoverPanel.activeInHierarchy;
    set => hoverPanel.SetActive(value);
  }
  
  private void Start()
  {
    Debug.Log(EnvReader.GetValue("TEST123"));
    hoverPanel = hoverCanvas.gameObject.transform.Find("Product Details Panel").gameObject;
    IsHoverPanelActive = false;
  }

  private void Update()
  {
    HandleHoverProduct();

    HandleRotation();

  }

  private void HandleRotation()
  {
    float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivityX;
    float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivityY;

    yRotation += mouseX;

    xRotation -= mouseY;
    xRotation = Mathf.Clamp(xRotation, -90f, 90f);

    transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    orientation.rotation = Quaternion.Euler(0, yRotation, 0);
  }

  private void HandleHoverProduct()
  {
    if(rayRenderer == null)
    {
      RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, 10f);
      for (int i = 0; i < hits.Length; i++)
      {
        RaycastHit hit = hits[i];
        Transform hitTransform = hit.transform;
        Renderer rendererLocal = hitTransform.GetComponent<Renderer>();
        
        if (rendererLocal != null)
        {
          GameObject rendererGO = rendererLocal.gameObject;

          if (rendererGO.CompareTag("keyboard"))
          {
            if(!IsHoverPanelActive)
            {
              IsHoverPanelActive = true;
            }

            foreach (JSONNode product in CMSProducts.products)
            {
              string sku = product["sku"];

              if (sku == rendererGO.name)
              {
                int id = product["id"];
                string name = product["name"];
                string price = product["price"];
                string imageURL = product["images"][0]["src"];

                if (CheckoutDetails.ProductTitle != name)
                {
                  CheckoutDetails.SetProductDetails(id, name, price, imageURL);
                }
              }
            }

            if (Input.GetKey(KeyCode.E))
            {
              CMSProducts.pickedProductInitialPosition = rendererGO.transform.position;
              CMSProducts.pickedProductInitialRotation = rendererGO.transform.rotation;
              isProductPickedUp = true;
              rayRenderer = rendererLocal;
              IsHoverPanelActive = false;
            }
          }
          else if (IsHoverPanelActive)
          {
            IsHoverPanelActive = false;
          }
        }
      }
    }
    else
    {
      Vector3 position = Camera.main.transform.position + Camera.main.transform.forward * 1.9f + Camera.main.transform.up * -0.8f;
      Quaternion rotation = Camera.main.transform.rotation * Quaternion.Euler(-90f, 0f, -90f);
      rayRenderer.transform.SetPositionAndRotation(position, rotation);

      HandleHoverShelf();
    }
  }

  private void HandleHoverShelf()
  {
    if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit))
    {
      Transform hitTransform = hit.transform;
      Renderer rendererLocal = hitTransform.GetComponent<Renderer>();
      GameObject rendererGO = rendererLocal.gameObject;

      if (rendererGO.CompareTag("shelf") && rayRenderer && isProductPickedUp && Input.GetKey(KeyCode.E))
      {
        isProductPickedUp = false;
        rayRenderer.transform.SetPositionAndRotation(CMSProducts.pickedProductInitialPosition, CMSProducts.pickedProductInitialRotation);
        rayRenderer = null;
      }
    }
  }
}
