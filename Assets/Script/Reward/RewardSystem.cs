using UnityEngine;

public class RewardSystem : MonoBehaviour
{
    public GameObject collectParticlePrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Reward"))
        {
            GameManager.instance.AddScore(10);
            Instantiate(collectParticlePrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Debug.Log("Score: " + GameManager.instance.scoreText.text);
            GameManager.instance.RespawnCoin();  // Respawn a new coin after collecting the coin
        }
    }
}
