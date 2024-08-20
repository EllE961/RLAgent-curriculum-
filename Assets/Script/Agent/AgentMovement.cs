using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class AgentMovement : Agent
{
    public GameObject area;
    public GameObject collisionParticlePrefab;
    public GameObject collectParticlePrefab;
    public float moveSpeed = 2;
    public float turnSpeed = 300;
    private Rigidbody m_AgentRb;
    private Vector3 m_StartingPosition;
    private CoinCollectorArea m_MyArea;
    private RayPerceptionSensorComponent3D rayPerception;

    public override void Initialize()
    {
        m_AgentRb = GetComponent<Rigidbody>();
        m_StartingPosition = transform.position;

        if (area == null)
        {
            Debug.LogError("Area GameObject is not assigned.");
            return;
        }

        m_MyArea = area.GetComponent<CoinCollectorArea>();
        if (m_MyArea == null)
        {
            Debug.LogError("CoinCollectorArea component is missing from the area GameObject.");
            return;
        }

        rayPerception = GetComponent<RayPerceptionSensorComponent3D>();
        if (rayPerception == null)
        {
            Debug.LogError("RayPerceptionSensorComponent3D is not attached to the agent.");
        }

        Debug.Log("Initialization complete.");
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        if (m_AgentRb == null)
        {
            Debug.LogError("Rigidbody is not assigned.");
            return;
        }

        if (m_MyArea == null)
        {
            Debug.LogError("CoinCollectorArea is not assigned.");
            return;
        }

        // Get the agent's velocity in its local coordinate system
        var localVelocity = transform.InverseTransformDirection(m_AgentRb.velocity);

        // Add the x and z components of the velocity to the observations
        sensor.AddObservation(localVelocity.x);
        sensor.AddObservation(localVelocity.z);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        MoveAgent(actionBuffers.DiscreteActions);
        AddReward(-1f / MaxStep);
    }

    private void MoveAgent(ActionSegment<int> act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        var action = act[0];
        switch (action)
        {
            case 1:
                dirToGo = transform.forward * 1f;
                break;
            case 2:
                dirToGo = transform.forward * -1f;
                break;
            case 3:
                rotateDir = transform.up * 1f;
                break;
            case 4:
                rotateDir = transform.up * -1f;
                break;
        }
        transform.Rotate(rotateDir, Time.fixedDeltaTime * turnSpeed);
        m_AgentRb.AddForce(dirToGo * moveSpeed, ForceMode.VelocityChange);

        ClampVelocity(5f);
    }

    private void ClampVelocity(float maxVelocity)
    {
        if (m_AgentRb.velocity.sqrMagnitude > maxVelocity * maxVelocity)
        {
            m_AgentRb.velocity *= 0.95f;
        }
    }

    public override void OnEpisodeBegin()
    {
        if (m_AgentRb == null || m_MyArea == null)
        {
            Debug.LogError("Initialization not completed correctly. Cannot reset episode.");
            return;
        }

        // Reset agent's position and velocity
        ResetPosition(m_StartingPosition);
        m_MyArea.ResetArea(new GameObject[] { gameObject });
    }

    private void ResetPosition(Vector3 position)
    {
        transform.position = position;
        m_AgentRb.velocity = Vector3.zero;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut.Clear();

        if (Input.GetKey(KeyCode.W))
        {
            discreteActionsOut[0] = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            discreteActionsOut[0] = 2;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            discreteActionsOut[0] = 4;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[0] = 3;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("coin"))
        {
            // Reward for collecting a coin
            AddReward(1.0f);
            Instantiate(collectParticlePrefab, collision.transform.position, Quaternion.identity);
            collision.gameObject.GetComponent<ObjectLogic>().OnEaten(); // Handle coin collection logic
        }
        if (collision.gameObject.CompareTag("obstacle"))
        {
            // Penalty for hitting an obstacle
            AddReward(-1.0f);
            Instantiate(collisionParticlePrefab, collision.transform.position, Quaternion.identity);
            collision.gameObject.GetComponent<ObjectLogic>().OnEaten(); // Handle obstacle logic if needed
        }
        if (collision.gameObject.CompareTag("wall"))
        {
            // Penalty for hitting a wall
            AddReward(-1.0f);
            Instantiate(collisionParticlePrefab, transform.position, Quaternion.identity);
        }
    }
}
