using System.Collections.Generic;
using UnityEngine;

public class QLearningAgent : MonoBehaviour
{
    private Dictionary<Vector3, float> qTable = new Dictionary<Vector3, float>();
    private float learningRate = 0.1f;
    private float discountFactor = 0.99f;
    private float explorationRate = 1.0f;
    private float explorationDecay = 0.99f;

    public GameObject rewardPrefab;
    public GameObject obstaclePrefab;
    public GameObject collectParticlePrefab;
    public GameObject collisionParticlePrefab;
    public GameObject destroyParticlePrefab;

    private void Start()
    {
        // Initialize Q-table
        InitializeQTable();
    }

    private void InitializeQTable()
    {
        // Initialize Q-table with random values or zeros
        foreach (var state in GetAllStates())
        {
            qTable[state] = 0.0f;
        }
    }

    private List<Vector3> GetAllStates()
    {
        // Generate all possible states (positions) in the environment
        List<Vector3> states = new List<Vector3>();
        // Example: For simplicity, consider a grid of positions
        for (int x = -10; x <= 10; x++)
        {
            for (int z = -10; z <= 10; z++)
            {
                states.Add(new Vector3(x, 0, z));
            }
        }
        return states;
    }

    private void Update()
    {
        // Q-learning update loop
        Vector3 currentState = transform.position;
        Vector3 nextState = GetNextState(currentState);

        float reward = GetReward(nextState);
        float qValue = qTable[currentState];
        float maxQValueNextState = GetMaxQValue(nextState);

        qTable[currentState] = qValue + learningRate * (reward + discountFactor * maxQValueNextState - qValue);

        // Move the agent to the next state
        transform.position = nextState;

        // Reduce exploration rate over time
        explorationRate *= explorationDecay;
    }

    private Vector3 GetNextState(Vector3 currentState)
    {
        // Epsilon-greedy policy for action selection
        if (Random.value < explorationRate)
        {
            // Explore: choose a random next state
            return GetRandomNextState(currentState);
        }
        else
        {
            // Exploit: choose the best next state based on Q-values
            return GetBestNextState(currentState);
        }
    }

    private Vector3 GetRandomNextState(Vector3 currentState)
    {
        // Choose a random next state (example: random position in the vicinity)
        Vector3[] possibleMoves = new Vector3[]
        {
            currentState + Vector3.forward,
            currentState + Vector3.back,
            currentState + Vector3.left,
            currentState + Vector3.right
        };

        return possibleMoves[Random.Range(0, possibleMoves.Length)];
    }

    private Vector3 GetBestNextState(Vector3 currentState)
    {
        // Choose the best next state based on Q-values
        Vector3 bestNextState = currentState;
        float maxQValue = float.MinValue;

        foreach (var state in GetRandomNextState(currentState))
        {
            if (qTable[state] > maxQValue)
            {
                maxQValue = qTable[state];
                bestNextState = state;
            }
        }

        return bestNextState;
    }

    private float GetReward(Vector3 state)
    {
        // Define the reward function
        // Example: +10 for collecting a reward, -10 for hitting an obstacle
        Collider[] colliders = Physics.OverlapSphere(state, 0.5f);

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Reward"))
            {
                return 10.0f;
            }
            if (collider.CompareTag("Obstacle"))
            {
                return -10.0f;
            }
        }

        return -1.0f; // Small penalty for each move to encourage shorter paths
    }

    private float GetMaxQValue(Vector3 state)
    {
        // Get the maximum Q-value for the given state
        float maxQValue = float.MinValue;

        foreach (var nextState in GetRandomNextState(state))
        {
            if (qTable[nextState] > maxQValue)
            {
                maxQValue = qTable[nextState];
            }
        }

        return maxQValue;
    }
}
