using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Vector3 position;
    public float speed = 1f;
    private float heuristicSpeed = 0.7f;
    private float noiseLevel = 1f;

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
        this.transform.localPosition += Vector3.forward * Time.deltaTime * speed;
        
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.localPosition += Vector3.forward * Time.deltaTime * heuristicSpeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.localPosition += Vector3.left * Time.deltaTime * heuristicSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.localPosition += Vector3.back * Time.deltaTime * heuristicSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.localPosition += Vector3.right * Time.deltaTime * heuristicSpeed;
        }
        
        position = this.transform.localPosition;
    }

    public Vector2 GetPosition()
    {
        return new Vector2(this.transform.position.x, this.transform.position.z);
    }
}
