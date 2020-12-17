using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerBehaviour : MonoBehaviour
{
    private Rigidbody2D rb;
    [Space]
    public bool onGround = false;
    public LayerMask platformLayers;
    public float groundDistance; // raycast distance between feet and ground
    float lastTapTime;

    private DistanceJoint2D distanceJoint;
    [SerializeField] private float SensorToPull = 0.6f;
    private bool toPullBool = false;
    public bool globalPullBool { get { return toPullBool; } }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        distanceJoint = GetComponent<DistanceJoint2D>();
        distanceJoint.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        OnGroundBehaviour();

        if (Input.GetKeyDown(KeyCode.Space) && onGround)
        {
            SwitchGravity();
        }


        ToPull();
    }


    public void SwitchGravity()
    {
        gravityEffect.instance.SwitchGravity(rb);
    }

    void OnGroundBehaviour()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, groundDistance, platformLayers);
        Debug.DrawLine(transform.position, transform.position - (transform.up * groundDistance), Color.blue);

        if (hit.collider != null)
        {
            onGround = true;
           //transform.up = hit.normal;
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
