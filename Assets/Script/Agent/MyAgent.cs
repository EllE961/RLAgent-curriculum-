using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class MyAgent : Agent
{
    public Transform target;
    public override void OnEpisodeBegin()
    {
        // Reset the agent and target positions
        if (transform.localPosition.y < 0)
        {
            transform.localPosition = new Vector3(0, 0.5f, 0);
        }
        target.localPosition = new Vector3(Random.Range(-5.0f, 5.0f), 0.5f, Random.Range(-5.0f, 5.0f));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Collect agent and target positions
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(target.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Apply actions
        var moveX = actionBuffers.ContinuousActions[0];
        var moveZ = actionBuffers.ContinuousActions[1];
        transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * 2.0f;

        // Rewards
        float distanceToTarget = Vector3.Distance(transform.localPosition, target.localPosition);

        // Reached target
        if (distanceToTarget < 1.42f)
        {
            SetReward(1.0f);
            EndEpisode();
        }

        // Fell off platform
        if (transform.localPosition.y < 0)
        {
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // Manual input for testing
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }
}
