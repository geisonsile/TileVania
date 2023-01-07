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

    public ParticleSystem dieEnemyFX;

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
        if (collision.tag == "Enemy")
        {
            AudioSource.PlayClipAtPoint(enemySFX, Camera.main.transform.position);
            Destroy(collision.gameObject);

            Instantiate(dieEnemyFX);
            dieEnemyFX.transform.localPosition = collision.gameObject.transform.localPosition;// - new Vector3(0, 0.5f, 0);
            print(collision.gameObject.transform.localPosition + collision.gameObject.name);
            //Physics2D.IgnoreCollision(collision, GetComponent<Collider2D>());
        }

        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
