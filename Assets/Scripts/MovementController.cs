using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController: MonoBehaviour
{

    public float movementSpeed = 0.5F;
    private float movementForce = 10f;
    private float rotationForce = 10f;
    public float rotationSpeed = 120F;
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

    public void MovePosition(Vector3 direction)
    {
        // deprecated, replaced by Move()
        moveDirection = direction * movementSpeed;
        moveDirection = transform.TransformDirection(moveDirection);
        this.transform.position += movementSpeed * moveDirection * Time.deltaTime;
    }

    public void Rotate(float rotationDegrees)
    {
        rb.AddTorque(0, rotationForce * rotationDegrees * Time.deltaTime, 0);
        transform.Rotate(0, rotationDegrees * rotationSpeed * Time.deltaTime, 0);
    }

    public void RotatePosition(float rotationDegrees)
    {
        // deprecated, replaced by Rotate()
        transform.Rotate(0, rotationDegrees * rotationSpeed * Time.deltaTime, 0);
    }

}

