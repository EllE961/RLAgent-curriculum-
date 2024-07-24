using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public TextMeshProUGUI scoreText;
    public GameObject coinPrefab;  // Prefab for the coin
    public Transform coinParent;   // Parent transform for organizing spawned coins
    public GameObject agentPrefab; // Prefab for the agent
    private GameObject agentInstance;
    private AgentMovement agentMovement; // Reference to the agent's movement script
    private int score = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateScoreText();
        SpawnAgent();
        SpawnCoin();
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    public void RestartGame()
    {
        StartCoroutine(RestartAfterDelay(1f));
    }

    private IEnumerator RestartAfterDelay(float delay)
    {
        agentMovement.EnableMovement(false);  // Disable agent movement
        yield return new WaitForSeconds(delay);
        ResetGame();  // Reset the game state
    }

    private void ResetGame()
    {
        // Reset the score
        ResetScore();

        // Reset the agent
        Destroy(agentInstance);
        SpawnAgent();

        // Respawn a new coin
        foreach (Transform child in coinParent)
        {
            Destroy(child.gameObject);
        }
        SpawnCoin();
    }

    private void ResetScore()
    {
        score = 0;
        UpdateScoreText();
    }

    public void RespawnCoin()
    {
        SpawnCoin();  // Spawn a new coin at a random location
    }

    private void SpawnCoin()
    {
        // Randomly spawn the coin within a specified range
        Vector3 randomPosition = new Vector3(Random.Range(-8.0f, 8.0f), 0.5f, Random.Range(-8.0f, 8.0f));
        Instantiate(coinPrefab, randomPosition, Quaternion.identity, coinParent);
    }

    private void SpawnAgent()
    {
        // Spawn the agent at the start position
        Vector3 startPosition = new Vector3(0, 0.5f, 0);
        agentInstance = Instantiate(agentPrefab, startPosition, Quaternion.identity);
        agentMovement = agentInstance.GetComponent<AgentMovement>();
    }
}
