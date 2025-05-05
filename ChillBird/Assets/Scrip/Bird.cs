using UnityEngine;
using System.Collections.Generic;

public class Bird : MonoBehaviour
{
    public float maxDragDistance = 3f;
    public float maxSpeed = 10f;
    public float gravity = 9.8f;

    public float stepTime = 0.02f;
    public int trajectorySteps = 20;

    private Vector2 startPosition;
    private Vector2 dragVector;
    private float speed;
    private float angle; // radians
    private bool isFlying = false;
    private float timeSinceLaunch = 0;

    private Camera cam;
    private LineRenderer line;
    private Rigidbody2D rb;
    public FollowCamera followCamera;

    void Start()
    {
        cam = Camera.main;
        line = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody2D>();
        if (cam != null && followCamera == null)
        {
            followCamera = cam.GetComponent<FollowCamera>();
            cam.transform.position = new Vector3(startPosition.x, startPosition.y, cam.transform.position.z);
        }
        
        line.startWidth = 0.1f;
        line.endWidth = 0.1f;
        line.positionCount = 0;
        startPosition = transform.position;
    }

    void OnMouseDown()
    {
        if (isFlying) return;
        startPosition = transform.position;
    }

    void OnMouseDrag()
    {
        if (isFlying) return;

        Vector2 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        dragVector = mouseWorld - startPosition;

        if (dragVector.magnitude > maxDragDistance)
        {
            dragVector = dragVector.normalized * maxDragDistance;
        }

        transform.position = startPosition + dragVector;

        speed = (dragVector.magnitude / maxDragDistance) * maxSpeed;
        angle = Mathf.Atan2(-dragVector.y, -dragVector.x);
        
        DrawTrajectory(startPosition, speed, angle);
    }

    void OnMouseUp()
    {
        if (isFlying) return;

        isFlying = true;
        timeSinceLaunch = 0;
        line.positionCount = 0;
        followCamera.isFollowing = true;
    }

    void FixedUpdate()
    {
        if (isFlying)
        {
            timeSinceLaunch += Time.fixedDeltaTime;

            Vector2 newPos = CalculatePosition(timeSinceLaunch, startPosition);
            transform.position = new Vector3(newPos.x, newPos.y, 0);

            if (transform.position.y < -10f)
            {
                ResetBird();
            }
        }
    }

    Vector2 CalculatePosition(float t, Vector2 origin)
    {
        float vx = speed * Mathf.Cos(angle);
        float vy = speed * Mathf.Sin(angle);
        
        float x = origin.x + vx * t;
        float y = origin.y + vy * t - 0.5f * gravity * t * t;
        
        return new Vector2(x, y);
    }

    void DrawTrajectory(Vector2 origin, float _speed, float _angle)
    {
        line.positionCount = trajectorySteps;

        for (int i = 0; i < trajectorySteps; i++)
        {
            
            float t = i * stepTime;
            
            Vector2 drawPos = CalculatePosition(t, origin);

            if (drawPos.y > -10)
            {
                line.SetPosition(i, drawPos);
            }
        }
    }

    void ResetBird()
    {
        isFlying = false;
        transform.position = startPosition;
        timeSinceLaunch = 0;
        
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        line.positionCount = 0;
        
        followCamera.isFollowing = false;

        cam.transform.position = new Vector3(startPosition.x, startPosition.y, cam.transform.position.z);
        
        line.positionCount = 0;
        

    }
}
