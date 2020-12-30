using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class playerBehaviour : MonoBehaviour
{
    private Rigidbody2D rb;

    public event EventHandler OnGravityEffect;

    public float FallDamageHeight;

    [Header("On Ground Settings")]
    public bool onGround = false;
    public LayerMask platformLayers;
    public float groundDistance; // raycast distance between feet and ground
    float lastTapTime;

    [Header("Pull Settings")]
    private bool toPullBool = false;
    public bool globalPullBool { get { return toPullBool; } }
    private DistanceJoint2D distanceJoint;
    [SerializeField] private float SensorToPull = 0.6f;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        distanceJoint = GetComponent<DistanceJoint2D>();
        distanceJoint.enabled = false;

        OnGravityEffect += PlayerBehaviour_OnGravityEffect; //Subscribe to Eventhandler
    }


    void Update()
    {
        OnGroundBehaviour();
        ToPull();


        if (Input.GetKeyDown(KeyCode.Space) && onGround)
        {
            OnGravityEffect(this,EventArgs.Empty);
        }
    }

    private void PlayerBehaviour_OnGravityEffect(object sender, EventArgs e)
    {
        SwitchingGravityEffect.instance.Switch(rb, true);
    }

    void OnGroundBehaviour()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, groundDistance, platformLayers);
        Debug.DrawLine(transform.position, transform.position - (transform.up * groundDistance), Color.blue);

        if (hit.collider != null)
        {
            onGround = true;
            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        }
        else
        {
            onGround = false;
        }
    }

    void ToPull()
    {
        RaycastHit2D hitBoxFront = Physics2D.Raycast(transform.position, transform.right, SensorToPull);

        if (Input.GetKey(KeyCode.E) && hitBoxFront && onGround)
        {
            toPullBool = true;

            if (hitBoxFront.collider.gameObject.CompareTag("Crate"))
            {
                distanceJoint.enabled = true;
                distanceJoint.connectedBody = hitBoxFront.collider.GetComponent<Rigidbody2D>();
            }
        }
        else
        {
            distanceJoint.connectedBody = null;
            distanceJoint.enabled = false;

            toPullBool = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + (transform.right * SensorToPull));
    }
}
