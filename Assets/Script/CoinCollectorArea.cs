using UnityEngine;
using Unity.MLAgentsExamples;
using System.Collections.Generic;

public class CoinCollectorArea : Area
{
    public GameObject coinPrefab;
    public GameObject obstaclePrefab;
    public int numCoins;
    public int numObstacles;
    public bool respawnCoins;
    public float range;

    private List<GameObject> spawnedObjects = new List<GameObject>();

    void CreateObjects(int num, GameObject prefab, bool respawn)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject obj = Instantiate(prefab, new Vector3(Random.Range(-range, range), 1f, Random.Range(-range, range)) + transform.position, Quaternion.identity);
            var logic = obj.GetComponent<ObjectLogic>();
            if (logic != null)
            {
                logic.respawn = respawn;
                logic.myArea = this;
            }
            spawnedObjects.Add(obj);
        }
    }

    public void ResetArea(GameObject[] agents)
    {
        foreach (GameObject obj in spawnedObjects)
        {
            Destroy(obj);
        }
        spawnedObjects.Clear();

        foreach (GameObject agent in agents)
        {
            if (agent.transform.parent == gameObject.transform)
            {
                agent.transform.position = new Vector3(Random.Range(-range, range), 2f, Random.Range(-range, range)) + transform.position;
                agent.transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360)));
            }
        }

        CreateObjects(numCoins, coinPrefab, respawnCoins);
        CreateObjects(numObstacles, obstaclePrefab, false);
    }

    public override void ResetArea()
    {
        // Optionally implement any area-specific reset logic here
    }
}
