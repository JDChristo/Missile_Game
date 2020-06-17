using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;

    private Transform target = null;
    private Vector3 offSet;

    void FixedUpdate()
    {
        if (target != null)
        {
            //transform.position = Vector3.Lerp(transform.position, target.position - offSet, speed * Time.deltaTime);

            Vector3 smoothedPos = Vector3.Lerp(transform.position, target.transform.position, speed);

            transform.position = new Vector3(smoothedPos.x, smoothedPos.y, transform.position.z);
        }
        else
        {
            target = GameManager.Instance.Target;
        }
    }
}
