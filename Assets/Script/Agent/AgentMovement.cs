using UnityEngine;

public class AgentMovement : MonoBehaviour
{
    public float speed = 5f;
    public float turnSpeed = 100f;
    public GameObject collisionParticlePrefab;
    public GameObject collectParticlePrefab;
    private bool canMove = true;

    void Update()
    {
        if (canMove)
        {
            float move = Input.GetAxis("Vertical") * speed * Time.deltaTime;
            float turn = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;

            transform.Translate(0, 0, move);
            transform.Rotate(0, turn, 0);
        }
    }

    public void EnableMovement(bool enable)
    {
        canMove = enable;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Instantiate(collisionParticlePrefab, transform.position, Quaternion.identity);
            Debug.Log("Hit an obstacle!");
            GameManager.instance.RestartGame();
            Destroy(gameObject);
        }
        else if (other.CompareTag("Reward"))
        {
            GameManager.instance.AddScore(10);
            Instantiate(collectParticlePrefab, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Debug.Log("Score: " + GameManager.instance.scoreText.text);
            GameManager.instance.RespawnCoin();  // Respawn a new coin after collecting the coin
        }
    }
}
