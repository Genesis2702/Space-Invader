using Unity.VisualScripting;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    private float xBound = 0.37f;
    private float backgroundBound = 11.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = SetInitialPosition();
    }

    // Update is called once per frame
    void Update()
    {
        float HorizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector2.right * speed *  HorizontalInput * Time.deltaTime);
        OutOfBounds();
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
}
