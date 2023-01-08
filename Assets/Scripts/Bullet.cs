using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 20f;
    [SerializeField] AudioClip enemySFX;

    Rigidbody2D myRigidbody;
    PlayerMovement player;
    float xSpeed;

    public GameObject dieEnemyFX;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
        xSpeed = player.transform.localScale.x * bulletSpeed;
    }

    
    void Update()
    {
        myRigidbody.velocity = new Vector2(xSpeed, 0f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy") // && collision.GetType().ToString().Equals("UnityEngine.CapsuleCollider2D"))
        {
            AudioSource.PlayClipAtPoint(enemySFX, Camera.main.transform.position);
            Destroy(collision.gameObject);
            
            GameObject effect = Instantiate(dieEnemyFX);
            effect.transform.position = collision.transform.position - new Vector3(0, 0.5f, 0);
        }

        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
