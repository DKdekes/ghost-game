using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class inherits GhostAgent and is used to train the search behavior in the agent.
 * At inference runtime (when we're going to play the game with a trained ghost), we won't
 * Be using this class. Instead we'll be infering from the GhostAgent class, and switching 
 * between a search model and a combat (shooting) model there.
 * 
 * To train the search behavior, drag this class into GhostAgent object's GhostAgent Script field.
 * Then in powershell run: mlagents-learn .\Assets\search_config.yaml --run-id=search-curriculum
 * */


public class SearchAgent : GhostAgent
{
    private float searchDistance = 10f;
    private int maxStepCount = 300000;
    private int stepCount = 0;

    public override void Shoot()
    {
        // don't shoot when searching
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
            // Debug.Log("timeout");
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
