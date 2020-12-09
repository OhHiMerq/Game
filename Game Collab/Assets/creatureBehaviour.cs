using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class creatureBehaviour : MonoBehaviour
{
    public LayerMask obstacleLayers;
    public bool idleMove
    {
        get { return _idleMove; }
        set
        {
            _idleMove = value;
            StopCoroutine("movingIdle");
            if (_idleMove)
            {
                StartCoroutine("movingIdle");
                //Debug.Log("Run the coroutine");
            }
                
            
        }
    }
    private bool _idleMove;

    private Rigidbody2D rb;
    [SerializeField] private float moveSpeed;
    public float travelMaxLength;
    public float cliffDetect;

    private RaycastHit2D cliffHit;

    [SerializeField] private bool faceRight = true;
    [Space]
    public float senseObstacleLength;
    public float senseObstacleOriginYOffset;
    private bool obstacleSensedLastUpdate;
    private bool obstacleSensed;
    [Space]
    public bool onGround = false;
    public LayerMask platformLayers;
    public float groundDistance; // raycast distance between feet and ground


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        idleMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            idleMove = !idleMove;
        }

        interfereObstacle();
        OnGroundBehaviour();
    }

    void interfereObstacle()
    {
        Vector2 originRay = (Vector2)transform.position;
        float sizeRay = senseObstacleLength;
        Vector3 directionRay = transform.right;

        RaycastHit2D leftA = Physics2D.Raycast(originRay + (Vector2)transform.up * -senseObstacleOriginYOffset, -directionRay, sizeRay, obstacleLayers);
        RaycastHit2D leftB = Physics2D.Raycast(originRay + (Vector2)transform.up * senseObstacleOriginYOffset, -directionRay, sizeRay, obstacleLayers);
        RaycastHit2D rightA = Physics2D.Raycast(originRay + (Vector2)transform.up * -senseObstacleOriginYOffset, directionRay, sizeRay, obstacleLayers);
        RaycastHit2D rightB = Physics2D.Raycast(originRay + (Vector2)transform.up * senseObstacleOriginYOffset, directionRay, sizeRay, obstacleLayers);


        Debug.DrawLine(originRay + (Vector2)transform.up * -senseObstacleOriginYOffset, (transform.position + transform.up * -senseObstacleOriginYOffset) - (transform.right * sizeRay), Color.yellow);/*leftA Raycast*/
        Debug.DrawLine(originRay + (Vector2)transform.up * senseObstacleOriginYOffset, (transform.position + transform.up * senseObstacleOriginYOffset) - (transform.right * sizeRay), Color.yellow);/*leftB Raycast*/
        Debug.DrawLine(originRay + (Vector2)transform.up * -senseObstacleOriginYOffset, (transform.position + transform.up * -senseObstacleOriginYOffset) + (transform.right * sizeRay), Color.yellow);/*rightA Raycast*/
        Debug.DrawLine(originRay + (Vector2)transform.up * senseObstacleOriginYOffset, (transform.position + transform.up * senseObstacleOriginYOffset) + (transform.right * sizeRay), Color.yellow);/*rightB Raycast*/


        /*Debug.DrawLine(transform.position, transform.position - (Quaternion.Euler(0, 0, 45) * transform.right * cliffDetect), Color.green);
        Debug.DrawLine(transform.position, transform.position - (Quaternion.Euler(0, 0, 135) * transform.right * cliffDetect), Color.green);*/

        if (leftA.collider != null || leftB.collider != null || rightA.collider != null || rightB.collider != null)
        {
            obstacleSensed = true;
        }
        else
        {
            obstacleSensed = false;
        }

        if (obstacleSensedLastUpdate == false && obstacleSensed)
        {
            idleMove = true;
            
        }
        obstacleSensedLastUpdate = obstacleSensed;
    }
    IEnumerator movingIdle() // yung gala gala lang
    {
        float interval = Random.Range(1.5f, 7f);
        Debug.Log("start new coroutine with " + interval + " interval");
        yield return new WaitForSeconds(interval);
        // shoot two rays, get two x values 
        Vector3 originRay = transform.position;
        Vector3 directionRay = Vector3.right; // hindi ito transform.right kasi pag nag rotate yung transform, pati yung ranges iikot 
        
        RaycastHit2D leftHit = Physics2D.Raycast(originRay, -directionRay, travelMaxLength,obstacleLayers);
        RaycastHit2D rightHit = Physics2D.Raycast(originRay, directionRay, travelMaxLength,obstacleLayers);

        Debug.DrawLine(transform.position, transform.position - (transform.right * travelMaxLength), Color.green);
        Debug.DrawLine(transform.position, transform.position + (transform.right * travelMaxLength), Color.green);

        float leftX = (originRay - (directionRay * travelMaxLength)).x;
        float rightX = (originRay + (directionRay * travelMaxLength)).x;
        if (leftHit.collider != null)
        {
            leftX = leftHit.point.x;
        }
        if (rightHit.collider != null)
        {
            rightX = rightHit.point.x;
        }

        // random between two x values
        float colliderBoundOffset = (GetComponent<Collider2D>().bounds.size.x / 2) + 0.5f;
        float targetX = Random.Range(leftX + colliderBoundOffset, rightX - colliderBoundOffset);

        // get the direction if left or right 
        float dir = Mathf.Sign(-(transform.position.x - targetX));


        if (dir > 0 && faceRight)
        {
            //WalkLeft Direction
            faceDirection();
        }
        else if (dir < 0 && !faceRight)
        {
            //WalkRight Direction
            faceDirection();
        }

        //Debug.Log("Enemy will go to: " + dir + " to: "+targetX+" from between " + leftX +" and " +rightX+"; with offset of "+colliderBoundOffset);
        // have a x velocity towards the direction 
        // if near to that x value na then assign new value for nextfire.
        while (!isPosPassedXTarget(dir,transform.position.x,targetX))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed * dir * Time.deltaTime, GetComponent<Rigidbody2D>().velocity.y);
            //Debug.Log("moving");
            yield return null;
        }



        idleMove = true;
        //Debug.Log("redo coroutine");

    }
    bool isPosPassedXTarget(float dir,float xPos,float xTarget)
    {
        if (dir > 0) // dir 1
        {
            return xPos >= xTarget;
        }
        else if(dir < 0) // dir -1
        {
            return xPos <= xTarget;
        }
        return false;
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

    void faceDirection()
    {
        faceRight = !faceRight;
        transform.Rotate(0, 180, 0);
    }
}
