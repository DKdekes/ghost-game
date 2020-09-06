using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class GhostAgent : Agent
{
    public GameObject boundaryPlane;
    public Transform playerTransform;
    private Vector3 playerStart;
    private MovementController movementController;
    private ShootProjectiles shooter;
    public GameObject origin;
    public GameObject barrel;
    private readonly object positionLock = new object();
    private float shootPunishment = 0.01f;
    private float reward = 0f;
    private bool resetting = false;
    float minX, maxX, minZ, maxZ, xScaler, zScaler;

    private void Awake()
    {
        SetBoundaries();
    }

    void Start()
    {
        this.playerStart = playerTransform.localPosition;
        this.movementController = GetComponent<MovementController>();
        this.shooter = GetComponent<ShootProjectiles>();
        Projectile.PlayerHit += Reward;
    }


    private void Reward()
    {
        reward += 1f;
        FinalizeReward();
        EndEpisode();
    }

    private void FinalizeReward()
    {
        SetReward(reward > 0 ? reward : 0);
    }

    public override void OnEpisodeBegin()
    {
        lock (this)
        {
            resetting = true;
            reward = 0f;
            this.transform.position = new Vector3(Random.Range(-3.5f, 3.5f), 1, Random.Range(-2f, 3.5f));
            this.transform.Rotate(new Vector3(0, Random.Range(-180f, 180f), 0));
            playerTransform.localPosition = new Vector3(Random.Range(-3.5f, 3.5f), 0.5f, -4.5f);
        }
    }

    private void SetBoundaries()
    {
        Vector3 scale = this.boundaryPlane.transform.localScale;
        Vector3 position = this.boundaryPlane.transform.position;
        float xHalf = scale.x * 5;
        float zHalf = scale.y * 5;
        this.minX = position.x - xHalf;
        this.maxX = position.x + xHalf;
        this.xScaler = this.maxX - this.minX;

        this.minZ = position.z - zHalf;
        this.maxZ = position.z + zHalf;
        this.zScaler = this.maxZ - this.minZ;
    }

    private (float, float) NormalizePosition(float x, float z)
    {
        float xNorm = (x - this.minX) / this.xScaler;
        float zNorm = (z - this.minZ) / this.zScaler;
        return (xNorm, zNorm);
    }


    public override void CollectObservations(VectorSensor sensor)
    {
        // agent position / rotation
        sensor.AddObservation(this.transform.localRotation.y / 180f);
        (float xNorm, float zNorm) = NormalizePosition(this.transform.localPosition.x, this.transform.localPosition.z);
        sensor.AddObservation(xNorm);
        sensor.AddObservation(zNorm);


        // shoot state
        sensor.AddObservation(shooter.GetShootReady());

        // player position 2d
        (float pXNorm, float pZNorm) = NormalizePosition(playerTransform.localPosition.x, playerTransform.localPosition.z);
        sensor.AddObservation(pXNorm);
        sensor.AddObservation(pZNorm);
        
    }

    public override void OnActionReceived(float[] act)
    {
        // walk
        int action = Mathf.FloorToInt(act[0]);

        if (action >= 0 && action <=3)
        {
            Move(action);
        }
        else if (action >= 4 && action <=5)
        {
            Rotate(action);
        }
        else if (action == 6)
        {
            Shoot();
        }
        // rewards
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, this.playerTransform.localPosition);

        // hit player
        if (distanceToTarget < 1.42f)
        {
            FinalizeReward();
            EndEpisode();
        }

        // out of bounds
        if (!InBounds(this.transform.localPosition))
        {
            FinalizeReward();
            EndEpisode();
        }
    }

    public override void Heuristic(float[] actionsOut)
    {
        // walk
        if (Input.GetKey(KeyCode.W))
        {
            actionsOut[0] = 0f;
        } else if (Input.GetKey(KeyCode.A))
        {
            actionsOut[0] = 1f;
        } else if (Input.GetKey(KeyCode.D)) {
            actionsOut[0] = 2f;
        } else
        {
            actionsOut[0] = 3f;
        }

        // rotate
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            actionsOut[1] = 0;
        } else if (Input.GetKey(KeyCode.RightArrow)) {
            actionsOut[1] = 1;
        } else
        {
            actionsOut[1] = 2;
        }

        // shoot
        if (Input.GetKey(KeyCode.Space))
        {
            actionsOut[2] = 0;
        } else
        {
            actionsOut[2] = 1;
        }
    }

    private bool InBounds(Vector3 position)
    {
        var playerPosition = this.playerTransform.localPosition;
        if ((position.x > -5 && position.x < 5) && (position.z > -5 && position.z < 5))
        {
            if ((playerPosition.x > -5 && playerPosition.x < 5) && (playerPosition.z > -5 && playerPosition.z < 5))
            {
                return true;
            }
        }
        return false;
    }

    private void Move(int direction)
    {
            // forward
            if (direction == 0) this.movementController.Move(Vector3.forward);
            // left
            if (direction == 1) this.movementController.Move(Vector3.left);
            // right
            if (direction == 2) this.movementController.Move(Vector3.right);

            // direction == 3 for staying still
    }

    private void Rotate(int direction)
    {
            float rotationStep = 0.1f;
            Quaternion rotation = Quaternion.identity;
            // rotate left
            if (direction == 3) this.movementController.Rotate(-rotationStep);
            // rotate right
            if (direction == 4) this.movementController.Rotate(rotationStep);
    }

    private void Shoot()
    {
        reward -= shootPunishment;
        shooter.Shoot(origin.transform.position, barrel.transform.position);
    }

}
