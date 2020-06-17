using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudBehaviour : MonoBehaviour
{
    public CloudManager cloudManager;
    void Update()
    {
        if (GameManager.Instance.Target)
        {
            if (Vector2.Distance(GameManager.Instance.Target.position, transform.position) > 15f)
            {
                cloudManager.RemoveCloud(gameObject);
                Destroy(gameObject);
            }
        }
    }
}
