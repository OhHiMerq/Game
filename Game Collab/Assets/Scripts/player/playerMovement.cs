using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    [SerializeField] private bool faceRight = true;
    [SerializeField] private float moveSpeed;
    private Vector3 moveDir;
    private Rigidbody2D rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
        if(Mathf.Abs(SimpleInput.GetAxis("Horizontal")) > 0)
        {
            moveDir.x = SimpleInput.GetAxis("Horizontal");
        } else if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0)
        {
            moveDir.x = Input.GetAxis("Horizontal");
        }


        

    }

    void FixedUpdate()
    {
        Move(moveDir);
    }

    void Move(Vector3 _moveDir) 
    {
        //rb.position += (Vector2)_moveDir * moveSpeed;
        rb.velocity = new Vector2(_moveDir.x * moveSpeed * Time.deltaTime,rb.velocity.y);

        if (moveDir.x < 0 && faceRight)
        {
            //WalkLeft Direction
            faceDirection();
        }
        else if (moveDir.x > 0 && !faceRight)
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
}
