using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Vector3 position;
    public float speed = 3f;

    public static bool IsHit(Vector3 projectilePosition, float maxRange)
    {
        if (Vector3.Distance(position, projectilePosition) <= maxRange)
        {
            return true;
        }
        return false;
    }

    private void Awake()
    {
        position = this.transform.position;
    }

    void Update()
    {

        this.transform.localPosition += Vector3.forward * Time.deltaTime * speed;
        /*
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.localPosition += Vector3.forward * Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.localPosition += Vector3.left * Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.localPosition += Vector3.back * Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.localPosition += Vector3.right * Time.deltaTime * speed;
        }
        */
        position = this.transform.localPosition;
    }

    public Vector3 GetPosition()
    {
        return this.transform.position;
    }
}
