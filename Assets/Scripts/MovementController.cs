using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController: MonoBehaviour
{

    public float movementSpeed = 0.5F;
    public float rotationSpeed = 120F;
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;

    // Use this for initialization
    void Start()
    {
        this.controller = GetComponent<CharacterController>();
    }

    public void Move(Vector3 direction)
    {
        // step player
        moveDirection = direction * movementSpeed;
        moveDirection = transform.TransformDirection(moveDirection);
        controller.Move(movementSpeed * moveDirection * Time.deltaTime);
    }

    public void Rotate(float rotationDegrees)
    { 
        //Rotate Player
        transform.Rotate(0, rotationDegrees * rotationSpeed * Time.deltaTime, 0);
    }

}

