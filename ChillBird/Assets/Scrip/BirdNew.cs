using UnityEngine;

public class BirdNew : MonoBehaviour
{
    public Camera cam;
    private Vector2 startPos;
    private Vector2 dragVector;
    private Rigidbody2D rb;

    public float maxDragDistance = 3f;
    public float forceMultiplier = 1000f;

    void Start()
    {
        //cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = 0;
        startPos = transform.position;
    }

    void OnMouseDown()
    {
        rb.gravityScale = 0;
        startPos = transform.position;
    }

    void OnMouseDrag()
    {
        PullSlingShot();
    }

    void OnMouseUp()
    {
        Move();
    }

    void Move()
    {
        rb.gravityScale = 1;

        Vector2 Direction = (startPos - (Vector2)transform.position).normalized;
        float Power = dragVector.magnitude * forceMultiplier;

        rb.AddForce(Direction * Power, ForceMode2D.Impulse);
    }

    void PullSlingShot()
    {
        Vector2 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        dragVector = mouseWorld - startPos;

        if (dragVector.magnitude > maxDragDistance)
        {
            dragVector = dragVector.normalized * maxDragDistance;
        }

        transform.position = startPos + dragVector;
    }
}