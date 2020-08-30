using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController: MonoBehaviour
{

    public float movementSpeed = 0.5F;
    public float rotationSpeed = 3.0F;
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;

    // Use this for initialization
    void Start()
    {
        this.controller = GetComponent<CharacterController>();
    }

    public void Move()
    {
        // step player
        moveDirection = new Vector3(0, 0, movementSpeed);
        moveDirection = transform.TransformDirection(moveDirection);
        controller.Move(movementSpeed * moveDirection * Time.deltaTime);
    }

    public void Rotate(float rotationDegrees)
    { 
        //Rotate Player
        transform.Rotate(0, rotationDegrees * rotationSpeed, 0);
    }

}

