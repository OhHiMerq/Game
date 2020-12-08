using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rb;

    public event EventHandler OnGravityEffect;

    [SerializeField] private float JumpForce = 5f;
    private bool CanJump = false;
    [SerializeField] private float MoveSpeed;
    private float _moveSpeed;
    private bool FacingRight = true;
    private Vector3 MoveDir;

    public Transform GroundChecker;
    [SerializeField]private float Radius;
    public LayerMask WhatisGround;
    private bool IsOnGround;

    private bool IsPulling;
    [SerializeField] private float SensorPush;
    public LayerMask Pushable;

    private float playerPullSpeed;

    [SerializeField] private float SensorSquash;

    private FixedJoint2D fixedJoint;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        fixedJoint = GetComponent<FixedJoint2D>();

        OnGravityEffect += PlayerMove_OnGravityEffect;

        _moveSpeed = MoveSpeed;
    }

    private void PlayerMove_OnGravityEffect(object sender, EventArgs e)
    {
        ChangeGravity();
    }

    


    private void Update()
    {
        MoveDir.x = Input.GetAxis("Horizontal");

        if (!IsOnGround)
        {
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }
        else
        {
            rb.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
        }


        if (Input.GetKeyDown(KeyCode.LeftShift) && IsOnGround)
        {
            //OnGravityEffect(this, EventArgs.Empty);
            ChangeGravity();
        }



        //PlayerJumpControl();
        ToPushing();
        ToPulling();
    }
    private void FixedUpdate()
    {
        IsOnGround = Physics2D.OverlapCircle(GroundChecker.position, Radius, WhatisGround);

        PlayerMovent(MoveDir);
        PlayerJumpForce();

        Tosquash();
    }


    private void ChangeGravity()
    {
        transform.Rotate(180, 0, 0);

        rb.gravityScale *= -1;
    }

    private void PlayerMovent(Vector3 MoveDirection)
    {
        rb.position += (Vector2)MoveDirection * _moveSpeed * Time.deltaTime;
        

        if (MoveDirection.x < 0 && FacingRight)
        {
            //WalkLeft Direction
            FaceDirection();
        }
        else if (MoveDirection.x > 0 && !FacingRight)
        {
            //WalkRight Direction
            FaceDirection();
        }
    }
    private void FaceDirection()
    {
        FacingRight = !FacingRight;
        transform.Rotate(0, 180, 0);
    }

    private void PlayerJumpControl()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsOnGround)
        {
            CanJump = true;
        }
        else if (!IsOnGround)
        {
            CanJump = false;
        }
    }
    private void PlayerJumpForce()
    {
        if (CanJump)
        {
            Vector2 velocity = rb.velocity;
            velocity.y = JumpForce * transform.up.y * Time.deltaTime;
            rb.velocity = velocity;
        }
    }

    private void ToPushing()
    {
        RaycastHit2D hit2D = Physics2D.Raycast(transform.position, transform.right, SensorPush, Pushable);
        

        float moveRaw = Input.GetAxisRaw("Horizontal");
        if (hit2D && (Pushable & 1 << hit2D.collider.gameObject.layer) == 1 << hit2D.collider.gameObject.layer)
        {
            if (moveRaw == transform.right.x && IsOnGround)
            {
                Crate CrateBox = hit2D.collider.GetComponent<Crate>();

                _moveSpeed = MoveSpeed - CrateBox.getPushSpeed;
                
            } else if (MoveDir.x == 0 || !IsOnGround)
            {
                _moveSpeed = MoveSpeed;
            }
        }
        else if (!IsPulling)
        {
            //Crate CrateBox = hit2D.collider.GetComponent<Crate>();
            //CrateBox.GetThisRB.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
            _moveSpeed = MoveSpeed;
        }
            
    }
    private void ToPulling()
    {
        float MoveRaw = Input.GetAxisRaw("Horizontal");

        if (Input.GetKey(KeyCode.E) && MoveRaw == -transform.right.x)
        {
            IsPulling = true;

            RaycastHit2D hit2D = Physics2D.Raycast(transform.position, transform.right, SensorPush, Pushable);

            if (hit2D && (Pushable & 1 << hit2D.collider.gameObject.layer) == 1 << hit2D.collider.gameObject.layer)
            {
                fixedJoint.enabled = true;
                fixedJoint.connectedBody = hit2D.rigidbody;

                Crate crateBox = hit2D.collider.GetComponent<Crate>();
                playerPullSpeed = MoveSpeed - crateBox.getPushSpeed;
                
            }
            _moveSpeed = playerPullSpeed;
        }
        else if (!IsOnGround)
        {
            fixedJoint.enabled = false;
            IsPulling = false;
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            fixedJoint.enabled = false;
            IsPulling = false;
        }
    }



    private void Tosquash()
    {
        bool Upper = Physics2D.Raycast(transform.position, transform.up, SensorSquash,WhatisGround);
        Debug.DrawLine(transform.position, transform.position + (transform.up * SensorSquash),Color.blue);

        bool Below = Physics2D.Raycast(transform.position, -transform.up, SensorSquash, WhatisGround);
        Debug.DrawLine(transform.position, transform.position + (-transform.up * SensorSquash),Color.blue);


        if (Upper && Below)
        {
            //Squash
            Death("Player's dies because of Squash");
        }
    }
    private void FallDamage(Collision2D col)
    {
        if(Mathf.Abs(col.relativeVelocity.y) > 30)
        {
            //FallDamage
            Death("Player's dies because of FallDamage");
        }
    }

    private void Death(string Deathmessage)
    {
        //Death
        Debug.Log(Deathmessage);
    }






    private void OnCollisionEnter2D(Collision2D collision)
    {
        FallDamage(collision);
        
    }





    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GroundChecker.position, Radius); //Visual circle for ground checker
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + (transform.right * SensorPush)); //Visual line for Pushing
    }

}