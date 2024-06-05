using UnityEngine;

public class PickUp : MonoBehaviour
{
  public GameObject player;
  public Transform holdPos;
  public float pickUpRange = 10f;

  readonly float rotationSensitivity = 1f;
  readonly float bigScreenDiagonalLengthDelimiter = 3.5f;
  readonly float bigScreenHoldPositionDistance = 2f;

  Vector3 initialHoldPosPosition;
  GameObject productGO;
  CameraController cameraController;
  PlayerController playerController;

  public Canvas hoverCanvas;
  GameObject hoverPanel;

  bool IsHoverPanelActive
  {
    get => hoverPanel.activeInHierarchy;
    set => hoverPanel.SetActive(value);
  }

  void Start()
  {
    cameraController = GetComponent<CameraController>();
    playerController = player.GetComponent<PlayerController>();

    initialHoldPosPosition = holdPos.localPosition;

    hoverPanel = hoverCanvas.gameObject.transform.Find($"Product Details Panel").gameObject;
    IsHoverPanelActive = false;
  }

  void Update()
  {
    // if products are loaded
    if (CMSProducts.products != null)
    {
      if (productGO == null) // if user doesn't hold anything
      {
        // handle hovering over the product
        HandleHoverProduct();
      }
      else // if user holds a product
      {
        // handle movement and rotation of the product
        MoveProduct();
        RotateProduct();

        // handle hovering over the raft
        HandleHoverRaft();
      }
    }
  }

  /// <summary>
  /// Triggers on frame update,
  /// when player doesn't hold a product.
  /// <para />
  /// Detect whether a product is visible or not in the
  /// area targeted by the mouse.
  /// </summary>
  void HandleHoverProduct()
  {
    // Because products are placed on shelves,
    // RaycastAll is used so the Ray can go
    // through the shelf itself
    RaycastHit[] hits = Physics.RaycastAll(
      transform.position,
      transform.TransformDirection(Vector3.forward),
      pickUpRange
    );

    foreach (RaycastHit hit in hits)
    {
      GameObject hitGO = hit.transform.gameObject;

      // If the targeted GameObject is a product, handle it
      if (hitGO.CompareTag("product"))
      {
        HoverProduct(hitGO);
      }
      // Disable panel if there's no product available
      else if (IsHoverPanelActive)
      {
        IsHoverPanelActive = false;
      }
    }
  }

  /// <summary>
  /// Activates when targeting a product with the mouse.
  /// <para />
  /// Display info for the hovered product
  /// and activate the pick up option.
  /// </summary>
  void HoverProduct(GameObject _productGO)
  {
    // In order to display the product info,
    // activate first the hover panel
    if (!IsHoverPanelActive)
    {
      IsHoverPanelActive = true;
    }

    // Loop through the products available
    foreach (var product in CMSProducts.products)
    {
      string sku = product.sku;

      // Find the one that we're currently looking
      // at based on its name (SKU identifier)
      if (sku == _productGO.name)
      {
        int id = product.id;
        string name = product.name;
        string price = product.price;
        string imageURL = product.images[0].src;

        // Save product data found and if it isn't already fetched,
        // set new data found in the Checkout Details
        // so we can use it later
        if (CheckoutDetails.productID != id)
        {
          CheckoutDetails.SetProductDetails(id, name, price, imageURL);
        }
      }
    }

    // Since we previously checked if the player doesn't have a product,
    // pressing E will pick up the product; also save its location and
    // rotation so we can later put it back on the shelf
    if (Input.GetKeyDown(KeyCode.E))
    {
      CMSProducts.pickedProductInitialPosition = _productGO.transform.position;
      CMSProducts.pickedProductInitialRotation = _productGO.transform.rotation;
      PickUpObject(_productGO);
      IsHoverPanelActive = false;
    }
  }

  /// <summary>
  /// Triggers when pressing E.
  /// <para />
  /// Set given product's position to the hold position.
  /// </summary>
  /// <param name="pickUpObj">The picked up product's Game Object</param>
  void PickUpObject(GameObject pickUpObj)
  {
    if (pickUpObj.GetComponent<Rigidbody>())
    {
      // Save product for later use
      productGO = pickUpObj;
      Rigidbody productGORb = pickUpObj.GetComponent<Rigidbody>();

      if (!productGORb.isKinematic)
      {
        productGORb.isKinematic = true;
      }

      // Place hold position further for big products
      // in order to have them properly displayed
      if (Functions.GetProductDiagonalLength(productGO) > bigScreenDiagonalLengthDelimiter)
      {
        UpdateHoldPositionZCoord(bigScreenHoldPositionDistance);
      }

      // Set layer so it overlaps the checkout desk and other objects,
      // preventing the product from going through elements
      Functions.SetLayerByName(productGO.transform, "Product");

      // Set product's position to player's hold position,
      // so it can follow the player around
      productGORb.transform.parent = holdPos.transform;

      // Disable product collision with player, sometimes causing bugs
      Physics.IgnoreCollision(
        productGO.GetComponent<Collider>(),
        player.GetComponent<Collider>(),
        true
      );

      // Disable cursor so user can properly see the product
      if (CursorController.ShowWebGLCursorCanvas)
      {
        CursorController.ShowWebGLCursorCanvas = false;
      }
    }
  }

  void UpdateHoldPositionZCoord(float value = 0)
  {
    holdPos.localPosition = initialHoldPosPosition + new Vector3(0, 0, value);
  }

  /// <summary>
  /// Detect wheter a raft has been hit or not in order to
  /// handle the drop product to initial position and rotation.
  /// </summary>
  void HandleHoverRaft()
  {
    // RaycastAll is used instead of Raycast because
    // when player has a product in hand, sometimes
    // the first object to be detected will be the product
    // and RaycastAll goes through all the objects detected.
    RaycastHit[] hits = Physics.RaycastAll(
      transform.position,
      transform.TransformDirection(Vector3.forward),
      pickUpRange
    );

    foreach (RaycastHit hit in hits)
    {
      GameObject hitGO = hit.transform.gameObject;

      if (hitGO.CompareTag("raft") && Input.GetKeyDown(KeyCode.E))
      {
        DropProduct();
      }
    }
  }

  /// <summary>
  /// Drop product back to its original position and rotation.
  /// <para />
  /// NOTE: Initial position and rotation are mandatory here.
  /// </summary>
  void DropProduct()
  {
    if (
      CMSProducts.pickedProductInitialPosition == null
      || CMSProducts.pickedProductInitialRotation == null
    )
    {
      return;
    }

    // Reset its layer so it doesn't appear through shelves
    Functions.SetLayerByName(productGO.transform, "Default");

    // Reset hold position Z Coord for big products
    if (Functions.GetProductDiagonalLength(productGO) > bigScreenDiagonalLengthDelimiter)
    {
      UpdateHoldPositionZCoord();
    }

    // Reset product to default values
    productGO.transform.SetPositionAndRotation(
      CMSProducts.pickedProductInitialPosition,
      CMSProducts.pickedProductInitialRotation
    );
    // Get rid of the references so that a new product can take its place
    productGO.transform.parent = null;
    productGO = null;

    // Reset product id so checkout canvas wont appear without a product in hand
    CheckoutDetails.productID = 0;

    // Re-enable player's cursor once he's puts the product back on the shelf
    if (!CursorController.ShowWebGLCursorCanvas && Checkout.step == 0)
    {
      CursorController.ShowWebGLCursorCanvas = true;
    }
  }

  /// <summary>
  /// When player holds a product, make sure to keep its position
  /// </summary>
  void MoveProduct()
  {
    // Keep object position the same as the holdPosition position
    productGO.transform.position = holdPos.transform.position;
  }

  /// <summary>
  /// Rotate held product.
  /// <para />
  /// When F key is held down, player's product can be rotated with mouse.
  /// This disables the ability to walk and rotate camera until R key is released.
  /// </summary>
  void RotateProduct()
  {
    if (Input.GetKey(KeyCode.F)) // Hold F key to rotate
    {
      // Disable player being able to look around and walk
      cameraController.canRotate = false;
      playerController.canMove = false;

      float XaxisRotation = Input.GetAxis("Mouse X") * rotationSensitivity;
      float YaxisRotation = Input.GetAxis("Mouse Y") * rotationSensitivity;

      // Rotate the object depending on mouse X-Y Axis
      productGO.transform.Rotate(Vector3.up, XaxisRotation);
      productGO.transform.Rotate(Vector3.left, YaxisRotation);
    }
    else
    {
      // Re-enable player being able to look around and walk
      cameraController.canRotate = true;
      playerController.canMove = true;
    }
  }
}
