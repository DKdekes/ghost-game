using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchAgent : GhostAgent
{
    private float searchDistance = 10f;
    private int maxStepCount = 20000;
    private int stepCount = 0;

    public override void Shoot()
    {
        // don't shoot
    }

    public override void OnEpisodeBegin()
    {
        base.OnEpisodeBegin();
        this.player.transform.localPosition = new Vector3(Random.Range(this.minX, this.maxX), 
            0.5f, Random.Range(this.minZ, this.maxZ));
        stepCount = 0;
        this.searchDistance = this.envParams.GetWithDefault("search_distance", 1f);
        this.player.SetSpeed(0f);
    }

    public override void CheckEpisodeEnd()
    {
        base.CheckEpisodeEnd();
        stepCount++;
        if (stepCount > maxStepCount)
        {
            Debug.Log("timeout");
            FinalizeReward(false);
            EndEpisode();
        }
 
        float distance = Vector3.Distance(this.transform.localPosition,
            this.player.transform.localPosition);
        if (distance < searchDistance)
        {
            this.reward += 1f;
            FinalizeReward(true);
            EndEpisode();
        }
    }

}
