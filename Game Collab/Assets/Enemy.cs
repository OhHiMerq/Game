using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    public Rigidbody2D MyRigid { get { return rb; } }
    private float SpriteSizeX;

    [SerializeField] private float MoveSpeed;
    private bool FacingRight = true;

    [Header("Cliff/Wall Sensor")]
    [SerializeField] private bool showLineSensor = false;
    public float CliffSensor = 1f, WallSensor = 0.5f;
    public LayerMask WallDetect;

    [Header("Squash Sensor")]
    public float SquashLenght;
    public LayerMask CanSquashMe;
    public float squashFeedbackStrength;

    [Header("On the way Sensor")]
    public float bumpSensorLength;
    public LayerMask CanBeBumped;
    public float bumpStrength;
    public float bumpPlayerTimeLength; // meron ganto kasi hindi magamitan ng add force yung player dahil may nakaset lagi sa velocity neto;

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        SpriteSizeX = GetComponent<SpriteRenderer>().bounds.extents.x;
    }

    public virtual void Update()
    {
        ImSquashed();
    }

    public virtual void FixedUpdate()
    {
        Movement();
        PushObjOnMyWay();
        SquashLiftObjDetector();
    }

    public virtual void Movement()
    {
        Vector2 velocity = rb.velocity;
        velocity.x = MoveSpeed * transform.right.x * Time.deltaTime;
        rb.velocity = velocity;
    }
    void EnemyFliper()
    {
        FacingRight = !FacingRight;
        transform.Rotate(0, 180, 0);
    }

    void Detector()
    {
        Vector3 DrawLineDir = transform.position + (transform.right * SpriteSizeX);

        bool DetectCliff = Physics2D.Linecast(DrawLineDir, DrawLineDir + (-transform.up * CliffSensor));
        bool DetectWall = Physics2D.Linecast(DrawLineDir, DrawLineDir + (transform.right * WallSensor), WallDetect);
        if (showLineSensor)
        {
            Debug.DrawLine(DrawLineDir, DrawLineDir + (-transform.up * CliffSensor), Color.red); //DetectCliff Visual
            Debug.DrawLine(DrawLineDir, DrawLineDir + (transform.right * WallSensor), Color.red); //DetectWall Visual
        }

        if (!DetectCliff || DetectWall)
        {
            EnemyFliper();
        }
    }


    public GameObject SquashLiftObjDetector()
    {
        RaycastHit2D detectTop = Physics2D.Raycast(transform.position, transform.up, SquashLenght, CanSquashMe);
        Debug.DrawLine(transform.position, transform.position + (transform.up * SquashLenght), Color.yellow);

        if (detectTop)
        {
            return detectTop.collider.gameObject;
        }

        return null;
    }
    public virtual void ImSquashed()
    {
        if (SquashLiftObjDetector() != null)
        {
            BounceObjThatSquashed(SquashLiftObjDetector());
            Death();
            
        }
        
    }

    void BounceObjThatSquashed(GameObject obj) // 1 
    {
        if (obj.GetComponent<Rigidbody2D>())
        {
            obj.GetComponent<Rigidbody2D>().AddForce(transform.up * squashFeedbackStrength,ForceMode2D.Impulse);
        }
    }

    

    public virtual void Death()
    {
        gameObject.SetActive(false);
    }

    public virtual void OnCollisionStay2D(Collision2D collision) //Act like Update but with colliders
    {
        if(collision != null)
        {
            Detector();
        }
    }

    void PushObjOnMyWay()
    {
        RaycastHit2D detectOnFace = Physics2D.Raycast(transform.position, transform.right, bumpSensorLength, CanBeBumped);
        Debug.DrawLine(transform.position, transform.position + (transform.right *bumpSensorLength), Color.green);

        if (detectOnFace.collider != null && detectOnFace.collider.gameObject.GetComponent<Rigidbody2D>())
        {
            Vector2 dir = (detectOnFace.point - (Vector2)transform.position).normalized;
            if (detectOnFace.collider.tag == "Player")
            {
                detectOnFace.collider.gameObject.GetComponent<playerMovement>().DisableMovementForTime(bumpPlayerTimeLength);
            }
            detectOnFace.collider.gameObject.GetComponent<Rigidbody2D>().AddForce(dir * bumpStrength * 10 * Time.deltaTime,ForceMode2D.Impulse);
        }
    }
    
}
