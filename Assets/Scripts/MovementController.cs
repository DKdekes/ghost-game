using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController: MonoBehaviour
{
    private float movementForce = 10f;
    private float rotationSpeed = GameSettings.rotationSpeed;
    private Vector3 moveDirection = Vector3.zero;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 direction)
    {
        direction = transform.TransformDirection(direction);
        rb.AddForce(direction * movementForce * Time.deltaTime);
    }

    public void Rotate(float direction)
    {
        transform.Rotate(0, direction * rotationSpeed * Time.deltaTime, 0);
    }

    public void RotatePosition(float rotationDegrees)
    {
        // deprecated, replaced by Rotate()
        transform.Rotate(0, rotationDegrees * rotationSpeed * Time.deltaTime, 0);
    }

    public void Begin()
    {
        rb.velocity = rb.angularVelocity = Vector3.zero;
    }

}

