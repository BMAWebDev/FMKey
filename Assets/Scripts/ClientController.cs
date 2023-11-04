using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientController : MonoBehaviour
{
  public float movementSpeed;

  public Transform orientation;

  float horizontalInput;
  float verticalInput;

  Vector3 moveDirection;

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
    // walk in the direction you're looking
    moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

    rb.AddForce(moveDirection.normalized * movementSpeed * 10f, ForceMode.Force);
  }
}
