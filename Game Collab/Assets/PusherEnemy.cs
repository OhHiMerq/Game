using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PusherEnemy : Enemy
{
    [Header("On the way Sensor")]
    public float bumpSensorLength;
    public LayerMask CanBeBumped;
    public float bumpStrength;
    public float bumpPlayerTimeLength; // meron ganto kasi hindi magamitan ng add force yung player dahil may nakaset lagi sa velocity neto;
    // Start is called before the first frame update
   
    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        PushObjOnMyWay();
    }

    public override void Movement()
    {
        base.Movement();
    }

    void PushObjOnMyWay()
    {
        RaycastHit2D detectOnFace = Physics2D.Raycast(transform.position, transform.right, bumpSensorLength, CanBeBumped);
        Debug.DrawLine(transform.position, transform.position + (transform.right * bumpSensorLength), Color.green);

        if (detectOnFace.collider != null && detectOnFace.collider.gameObject.GetComponent<Rigidbody2D>())
        {
            Vector2 dir = (detectOnFace.point - (Vector2)transform.position).normalized;
            if (detectOnFace.collider.tag == "Player")
            {
                detectOnFace.collider.gameObject.GetComponent<playerMovement>().DisableMovementForTime(bumpPlayerTimeLength);
            }
            detectOnFace.collider.gameObject.GetComponent<Rigidbody2D>().AddForce(dir * bumpStrength * 10 * Time.deltaTime, ForceMode2D.Impulse);
        }
    }
}
