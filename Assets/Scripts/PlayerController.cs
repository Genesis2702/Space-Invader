using System.Collections;
using Unity.VisualScripting;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private float speed = 5.0f;
    private float xBound = 0.37f;
    private float backgroundBound = 11.0f;
    public GameObject bulletPrefab;
    private int lives = 3;
    public bool isAlive;
    public bool isHit;
    private float fireRate = 0.5f;
    private bool shootAllow = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isAlive = true;
        isHit = false;
        transform.position = SetInitialPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive == true)
        {
            float HorizontalInput = Input.GetAxis("Horizontal");
            transform.Translate(Vector2.right * speed *  HorizontalInput * Time.deltaTime);
            OutOfBounds();
            if (Input.GetKeyDown(KeyCode.Space) && shootAllow)
            {
                StartCoroutine(Shooting());
            }
        }
        if (lives == 0)
        {
            isAlive = false;
        }
    }

    IEnumerator Shooting()
    {
        shootAllow = false;
        yield return new WaitForSeconds(fireRate);
        SpawningBullets();
        shootAllow = true;
    }

    private Vector2 SetInitialPosition()
    {
        Vector2 offset = new Vector2(0, -4);
        return new Vector2(0, 0) + offset;
    }

    private void OutOfBounds()
    {
        if (transform.position.x > backgroundBound - xBound)
        {
            transform.position = new Vector3(backgroundBound - xBound, transform.position.y);
        }
        else if (transform.position.x < -backgroundBound  + xBound)
        {
            transform.position = new Vector3(-backgroundBound + xBound, transform.position.y);
        }
    }

    private void SpawningBullets()
    {
        Instantiate(bulletPrefab, transform.position + new Vector3(0, 0.1f), bulletPrefab.gameObject.transform.rotation);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy Bullet"))
        {
            isHit = true;
            lives--;
        }
    }
}
