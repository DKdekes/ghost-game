using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class GhostAgent : Agent
{
    public Transform playerTransform;
    private Vector3 playerStart;
    void Start()
    {
        this.playerStart = playerTransform.localPosition;
    }

    public override void OnEpisodeBegin()
    {
        // set the agent to its starting position
        // we should randomize this start position in the future
        var x = Random.Range(-3.5f, 3.5f);
        var z = Random.Range(-2f, 3.5f);
        this.transform.position = new Vector3(x, 1, z);
        playerTransform.position = this.playerStart;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(playerTransform.localPosition);
        sensor.AddObservation(this.transform.localPosition);
    }

    public override void OnActionReceived(float[] act)
    {
        int movement = Mathf.FloorToInt(act[0]);
        Move(movement);

        // rewards
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, this.playerTransform.localPosition);

        // reached target
        if (distanceToTarget < 1.42f)
        {
            SetReward(1.0f);
            EndEpisode();
        }

        // out of bounds
        if (!InBounds(this.transform.localPosition)) {
            EndEpisode();
        }

    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Horizontal");
        actionsOut[1] = Input.GetAxis("Vertical");
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
        float step = 0.01f;
        Vector3 movement = Vector3.zero;
        // up
        if (direction == 1) movement.z = step;
        // down
        if (direction == 2) movement.z = -step;
        // left
        if (direction == 3) movement.x = -step;
        // right
        if (direction == 4) movement.x = step;
         
        this.transform.localPosition += movement;
    }

}
