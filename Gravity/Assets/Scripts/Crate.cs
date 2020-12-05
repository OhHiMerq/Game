using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : GravityEffect
{
    [SerializeField] private float PushSpeed = 5f;
    public float getPushSpeed { get { return PushSpeed; } }

    private bool StopPushing;
    public bool GetStopPushing { get { return StopPushing; } }

    [SerializeField] private float SensorLength = 1.05f;
    [SerializeField] private LayerMask WhereToLand;

    
    private void Update()
    {
        if(GetThisRB.velocity != Vector2.zero)
        {
            GetThisRB.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            
        } else if (GetThisRB.velocity == Vector2.zero)
        {
            GetThisRB.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
        }


        LockRotation();



    }

    //Boolean if the crate if landed
    private bool SensorLine()
    {
        bool Up = Physics2D.Raycast(transform.position, Vector2.up, SensorLength, WhereToLand);

        bool Down = Physics2D.Raycast(transform.position, Vector2.down, SensorLength, WhereToLand);

        bool Left = Physics2D.Raycast(transform.position, Vector2.left, SensorLength, WhereToLand);

        bool Right = Physics2D.Raycast(transform.position, Vector2.right, SensorLength, WhereToLand);

        if(Up || Down || Left || Right)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Lock the rotation if not falling
    private void LockRotation()
    {
        if (Mathf.Abs(GetThisRB.velocity.y) > 0.01f && !SensorLine())
        {
            GetThisRB.freezeRotation = false;
        }
        else if (Mathf.Abs(GetThisRB.velocity.y) < 0.01f && SensorLine())
        {
            GetThisRB.freezeRotation = true;
        }
        else
        {
            GetThisRB.freezeRotation = false;
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            

            foreach (ContactPoint2D points in collision.contacts)
            {
                if (Mathf.Abs(points.normal.x) > 0)
                {
                    //GetThisRB.constraints &= ~RigidbodyConstraints2D.FreezePositionX;

                    StopPushing = true;
                }
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            StopPushing = false;
        }
         
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.up * SensorLength));
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.down * SensorLength));
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.left * SensorLength));
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.right * SensorLength));
    }
}
