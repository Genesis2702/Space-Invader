using UnityEngine;

public class FreeFalling : MonoBehaviour
{
    private float gravitationalForce = 5.0f;
    private float yBound = 6.0f;
    private Rigidbody orbRb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        orbRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * gravitationalForce * Time.deltaTime);
        DestroyOutOfBound();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    private void DestroyOutOfBound()
    {
        if (transform.position.y < -yBound)
        {
            Destroy(gameObject);
        }
    }
}
