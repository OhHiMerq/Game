using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    private playerBehaviour PB;
    [SerializeField] private bool faceRight = true;
    [SerializeField] private float moveSpeed;
    private Vector2 moveDir;
    private Rigidbody2D rb;


    [Header("Disable Movement Velocity Influence")]
    public bool canMove;
    public bool willMove;







    void Start()
    {
        PB = GetComponent<playerBehaviour>();
        rb = GetComponent<Rigidbody2D>();
        canMove = true;
        Physics2D.queriesStartInColliders = false;
    }

    void Update()
    {
        moveDir.x = Input.GetAxisRaw("Horizontal");
        moveDir.x = SimpleInput.GetAxisRaw("Horizontal");

        //if(Mathf.Abs(SimpleInput.GetAxis("Horizontal")) > 0)
        //{
        //    moveDir.x = SimpleInput.GetAxis("Horizontal");
        //} else if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0)
        //{
        //    moveDir.x = SimpleInput.GetAxis("Horizontal");
        //}

    }
    void FixedUpdate()
    {
        if (canMove && willMove)
        {
            Move(moveDir);
        }

        
        if (Mathf.Abs(moveDir.x) > 0f || GetComponent<playerBehaviour>().onGround)
        {
            willMove = true;
        }
        else
        {
            willMove = false;
        }
    }


    void Move(Vector2 _moveDir) 
    {
        float speedX = _moveDir.x * moveSpeed * Time.deltaTime;
        rb.velocity = new Vector2(speedX, rb.velocity.y);

        if (moveDir.x < 0 && faceRight && !PB.globalPullBool)
        {
            //WalkLeft Direction
            faceDirection();
        }
        else if (moveDir.x > 0 && !faceRight && !PB.globalPullBool)
        {
            //WalkRight Direction
            faceDirection();
        }
    }
    void faceDirection()
    {
        faceRight = !faceRight;
        transform.Rotate(0, 180, 0);
    }


    public void DisableMovementForTime(float time)
    {
        StopCoroutine("disableMove");
        StartCoroutine(disableMove(time));
    }
    IEnumerator disableMove(float _time)
    {
        canMove = false;
        willMove = false;
        yield return new WaitForSeconds(_time);
        canMove = true;
    }
}
