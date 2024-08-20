using UnityEngine;

public class ObjectLogic : MonoBehaviour
{
    public bool respawn;
    public CoinCollectorArea myArea;

    public void OnEaten()
    {
        if (respawn)
        {
            Respawn();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Respawn()
    {
        transform.position = new Vector3(Random.Range(-myArea.range, myArea.range), 1f, Random.Range(-myArea.range, myArea.range)) + myArea.transform.position;
        transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360), 0f));
    }
}
