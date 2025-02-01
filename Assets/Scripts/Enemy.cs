using System.Collections;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float speed = 3.0f;
    private int backgroundBound = 11;
    private float xBound = 0.45f;
    public GameObject enemyBullet;
    private bool shootAllow = true;
    private float fireRate = 3.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        OutOfBoundsDirection();
        if (shootAllow == true)
        {
            StartCoroutine(ShootingBullet());
        }
    }

    IEnumerator ShootingBullet()
    {
        shootAllow = false;
        yield return new WaitForSeconds(fireRate);
        Instantiate(enemyBullet, transform.position, enemyBullet.transform.rotation);
        shootAllow = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
    }

    private void OutOfBoundsDirection()
    {
        if (transform.position.x > backgroundBound - xBound)
        {
            speed *= -1;
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        else if (transform.position.x < -backgroundBound + xBound)
        {
            speed *= -1;
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
    }
}
