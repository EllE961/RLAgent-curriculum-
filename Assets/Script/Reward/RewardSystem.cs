using UnityEngine;

public class RewardSystem : MonoBehaviour
{
    public GameObject collectParticlePrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Agent"))
        {
            GameManager.instance.AddScore(10);
            Instantiate(collectParticlePrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
            Debug.Log("Score: " + GameManager.instance.scoreText.text);
        }
    }
}
