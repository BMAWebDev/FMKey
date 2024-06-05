using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  public float movementSpeed;
  public Transform orientation;
  public bool canMove = true;

  float horizontalInput;
  float verticalInput;

  Rigidbody rb;

  void Start()
  {
    rb = GetComponent<Rigidbody>();
    rb.freezeRotation = true;
  }

  void Update()
  {
    UpdateInput();

    rb.drag = 5f;
  }

  void FixedUpdate()
  {
    MovePlayer();
  }

  void UpdateInput()
  {
    horizontalInput = Input.GetAxisRaw("Horizontal");
    verticalInput = Input.GetAxisRaw("Vertical");
  }

  void MovePlayer()
  {
    if (canMove && !Settings.isPaused)
    {
      // walk in the direction you're looking
      Vector3 moveDirection =
        orientation.forward * verticalInput + orientation.right * horizontalInput;
      rb.AddForce(10f * movementSpeed * moveDirection.normalized, ForceMode.Force);
    }
  }
}
