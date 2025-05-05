using System;
using Unity.VisualScripting;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public Transform maxPos;
    public Transform minPos;
    
    
    public bool isFollowing = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        //if(!isFollowing || target == null) return;
        
        Vector3 targetPos = target.position;
        targetPos.z = transform.position.z;
        
        Vector3 newPos = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * 10f);
        
        newPos.x = Mathf.Clamp(newPos.x, minPos.position.x, maxPos.position.x);
        
        newPos.y = Mathf.Clamp(newPos.y, minPos.position.y, maxPos.position.y);
        
        transform.position = newPos;
    }

    public void SetPosTargetToBird()
    {
        transform.position = target.position;
    }
}
