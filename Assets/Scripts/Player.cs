using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Vector3 position;
    public float speed = 1f;
    private float heuristicSpeed = 0.7f;
    private float noiseLevel = 1f;
    private Camera playerCam;
    Rigidbody m_Rigidbody;
    Vector3 m_EulerAngleVelocity;

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

        //finds the player camera 
        playerCam = GameObject.Find("PlayerCamera").GetComponent<Camera>();

        //Fetch the Rigidbody from the GameObject with this script attached
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    public float GetNoiseLevel()
    {
        return this.noiseLevel;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    void Update()
    {
        //looking
        //m_EulerAngleVelocity = new Vector3(0, Input.GetAxis("Mouse X") * 100, 0);
        //Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.deltaTime);
        //m_Rigidbody.MoveRotation(m_Rigidbody.rotation * deltaRotation);
        this.transform.Rotate(0, Input.GetAxis("Mouse X")*2, 0);
        playerCam.transform.Rotate(-Input.GetAxis("Mouse Y") * 2, 0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            this.transform.localPosition += this.transform.forward * Time.deltaTime * heuristicSpeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.localPosition += -this.transform.right * Time.deltaTime * heuristicSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.localPosition += -this.transform.forward * Time.deltaTime * heuristicSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.localPosition += this.transform.right * Time.deltaTime * heuristicSpeed;
        }
        
        position = this.transform.localPosition;
    }

    public Vector2 GetPosition()
    {
        return new Vector2(this.transform.position.x, this.transform.position.z);
    }
}
