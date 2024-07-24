using UnityEngine;

public class ObstacleCollision : MonoBehaviour
{
    public GameObject collisionParticlePrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Instantiate(collisionParticlePrefab, transform.position, Quaternion.identity);
            Debug.Log("Hit an obstacle!");
            Destroy(gameObject);
            GameManager.instance.RestartGame();
        }
    }
}
