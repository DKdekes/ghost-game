﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public delegate void Action();
    public static event Action PlayerHit;
    private Vector3 shootDir;
    public float speed = 1f;
    public float maxRange = 1f;

    public void Setup(Vector3 direction)
    {
        this.shootDir = direction;
        Destroy(gameObject, GameSettings.projectileTimeout);
    }

    public void Update()
    {
        transform.position += shootDir * Time.deltaTime * speed;
        if (Player.IsHit(this.transform.position, maxRange))
        {
            PlayerHit();
            Destroy(gameObject);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }


}
