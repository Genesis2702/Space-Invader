using UnityEngine;

public class RepeatingBackground : MonoBehaviour
{
    private float speed = 5.0f;
    private Vector2 centerPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        centerPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
        if (transform.position.y < centerPosition.y - 19.2f)
        {
            transform.position = centerPosition;
        }
    }
}
