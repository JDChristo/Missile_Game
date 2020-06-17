using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBehaviour : MonoBehaviour
{
    public Transform target;
    [HideInInspector]
    public float lifeTime;
    
    [SerializeField]
    private float moveSpeed = 6f;
    [SerializeField]
    private float acceleration = 3f;
    [SerializeField]
    private float angleSpeed = 180f;
    [SerializeField]
    private GameObject explosionEffect = null;

    private Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        Invoke("ExplodedMissile", lifeTime);
    }
    private void FixedUpdate()
    {
        MissileFollow();
    }

    public void MissileFollow()
    {
        rb.velocity = transform.up * moveSpeed;

        if (target != null)
        {
            Vector2 direction = rb.position - (Vector2)target.position;
            direction.Normalize();
            float angle = Vector3.Cross(direction, transform.up).z;
            rb.angularVelocity = angleSpeed * angle;
        }
        else
        {
            rb.angularVelocity = 0f;
        }

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            print("hit");
            Destroy(other.gameObject);
            ExplodedMissile();
            GameManager.Instance.GameOver(other.transform);
        }
        if (other.gameObject.tag.Equals("Missile"))
        {
            ExplodedMissile();
        }
    }

    void ExplodedMissile()
    {
        GameObject explosion = Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(explosion, 1.2f);
        Destroy(gameObject);
    }
}
