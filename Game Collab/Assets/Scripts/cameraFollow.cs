using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{

    public Transform cameraTarget;
    public Vector3 offset;

    [Range(0.01f,1f)]
    public float damp = 0.125f;
    
    private Vector3 velocity = Vector3.zero;

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetPos = cameraTarget.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position,targetPos,ref velocity, damp);
    }
}
