using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 5f;
    public Transform orientation;

    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        // Smooth out visual jitter by interpolating Rigidbody between physics steps
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.MovePosition(transform.position + moveDirection.normalized * speed * Time.fixedDeltaTime);
    }

}
