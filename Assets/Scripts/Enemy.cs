using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float speed = 4.0f;
    private int backgroundBound = 13;
    private float xBound = 0.45f;
    public GameObject enemyBullet;
    private bool shootAllow = true;
    private float fireRate = 3.0f;
    public List<GameObject> powerup;
    private PlayerController playerControllerScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerControllerScript.isAlive == true)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            OutOfBoundDirection();
            if (shootAllow == true)
            {
                StartCoroutine(ShootingBullet());
            }
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
            int chance = Random.Range(0, 100);
            if (chance >= 70)
            {
                int typeOfOrb = Random.Range(0, 2);
                Instantiate(powerup[typeOfOrb], transform.position, powerup[typeOfOrb].transform.rotation);
            }
        }
    }

    private void OutOfBoundDirection()
    {
        if (transform.position.x > backgroundBound - xBound || transform.position.x < -backgroundBound + xBound)
        {
            speed *= -1;
        }
    }
}
