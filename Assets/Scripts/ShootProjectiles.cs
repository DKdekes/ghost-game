using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectiles : MonoBehaviour
{

    [SerializeField] private Transform projectile;
    public int recoverFrames;
    bool shootReady = true;
    float frameCount = 0f;


    public bool GetShootReady()
    {
        return shootReady;
    }

    private void Update()
    {
        if (frameCount > 0)
        {
            frameCount--;
            if (frameCount <= 0)
            {
                shootReady = true;
            }
        }
    }

    public void Shoot(Vector3 origin, Vector3 barrel)
    {
        if (shootReady)
        {
            shootReady = false;
            frameCount = recoverFrames * Time.deltaTime;
            Transform projectileTransform = Instantiate(projectile, barrel, Quaternion.identity);
            Vector3 shootDir = (barrel - origin).normalized;
            projectileTransform.GetComponent<Projectile>().Setup(shootDir);
        }
    }
}
