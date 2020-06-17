using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour
{
    public int numOfCloud = 10;
    public GameObject[] cloudPrefab;

    private List<GameObject> cloudList = new List<GameObject>();
    void Update()
    {
        while (cloudList.Count < numOfCloud && GameManager.Instance.Target != null)
        {
            Vector2 direction = GameManager.Instance.PlaneDirection;
            int x = (direction.x > 0) ? Random.Range(0, 8) : Random.Range(0, -8);
            int y = (direction.y > 0) ? Random.Range(0, 8) : Random.Range(0, -8);

            direction = new Vector3(x, y);
            Vector2 spawnLocation = (Vector2)GameManager.Instance.Target.position + direction.normalized * 15f;
            GameObject cloud = Instantiate(cloudPrefab[Random.Range(0, cloudPrefab.Length)], spawnLocation, Quaternion.identity, transform);
            cloud.GetComponent<CloudBehaviour>().cloudManager = this;
            cloudList.Add(cloud);
        }
    }

    public void RemoveCloud(GameObject cloud)
    {
        cloudList.Remove(cloud);
    }
}
