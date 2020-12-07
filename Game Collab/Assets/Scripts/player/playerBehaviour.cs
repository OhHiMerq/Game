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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        OnGroundBehaviour();

        if (Input.GetKeyDown(KeyCode.Space) && GetComponent<playerBehaviour>().onGround)
        {
            SwitchGravity();

        }
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


}
