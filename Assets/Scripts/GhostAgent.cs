using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class GhostAgent : Agent
{
    public Transform playerTransform;
    private Vector3 playerStart;
    private MovementController movementController;
    private ShootProjectiles shooter;
    public GameObject origin;
    public GameObject barrel;
    private readonly object positionLock = new object();
    private float shootPunishment = 0.0001f;
    private float reward = 1f;
    private bool resetting = false;

    void Start()
    {
        this.playerStart = playerTransform.localPosition;
        this.movementController = GetComponent<MovementController>();
        this.shooter = GetComponent<ShootProjectiles>();
        Projectile.PlayerHit += Reward;
    }


    private void Reward()
    {
            SetReward(reward);
            EndEpisode();
    }

    public override void OnEpisodeBegin()
    {
        lock (this)
        {
            resetting = true;
            reward = 1f;
            this.transform.position = new Vector3(Random.Range(-3.5f, 3.5f), 1, Random.Range(-2f, 3.5f));
            this.transform.Rotate(new Vector3(0, Random.Range(-180f, 180f), 0));
            playerTransform.localPosition = new Vector3(Random.Range(-3.5f, 3.5f), 0.5f, -4.5f);
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // agent position / rotation
        sensor.AddObservation(this.transform.localRotation.y);
        sensor.AddObservation(this.transform.localPosition.x);
        sensor.AddObservation(this.transform.localPosition.z);

        // shoot state
        sensor.AddObservation(shooter.GetShootReady());

        // player position 2d
        sensor.AddObservation(playerTransform.localPosition.x);
        sensor.AddObservation(playerTransform.localPosition.z);
        
    }

    public override void OnActionReceived(float[] act)
    {
        lock(this)
        {
            // walk
            int movement = Mathf.FloorToInt(act[0]);
            Move(movement);

            // rotate
            int rotation = Mathf.FloorToInt(act[1]);
            Rotate(rotation);

            // shoot
            int shoot = Mathf.FloorToInt(act[2]);
            Shoot(shoot);

            // rewards
            float distanceToTarget = Vector3.Distance(this.transform.localPosition, this.playerTransform.localPosition);

            // hit player
            if (distanceToTarget < 1.42f)
            {
                EndEpisode();
            }

            // out of bounds
            if (!InBounds(this.transform.localPosition))
            {
                EndEpisode();
            }
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
            if (direction == 0) this.movementController.Rotate(-rotationStep);
            // rotate right
            if (direction == 1) this.movementController.Rotate(rotationStep);
    }

    private void Shoot(int shoot)
    {
        reward -= shootPunishment;
        if (shoot == 0)
        {
            shooter.Shoot(origin.transform.position, barrel.transform.position);
        }
    }

}
