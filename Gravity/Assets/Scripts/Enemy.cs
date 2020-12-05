using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D enemyRB;
    private float SpriteSize;

    [SerializeField] private float MoveSpeed = 11;
    [SerializeField] private bool Reverse = false;
    private bool FacingRight;

    [SerializeField] private LayerMask detectEdge;
    private float WallSensorWidth = 0.2f;
    

    [SerializeField] private LayerMask UseToSquash;
    private float SquashSensor = 1f;


    private void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        SpriteSize = GetComponent<SpriteRenderer>().bounds.extents.x;

        if (Reverse)
        {
            EnemyReverse();
        }
    }

    private void FixedUpdate()
    {
        EnemyMove();
        Tosquash();
    }

    private void EnemyReverse()
    {
        enemyRB.gravityScale = -1f;
        transform.Rotate(180, 0, 0);
    }
    private void EnemyMove()
    {
        Vector2 velocity = enemyRB.velocity;
        velocity.x = MoveSpeed * transform.right.x * Time.deltaTime;
        enemyRB.velocity = velocity;

        
    }
    private void EnemyFlip()
    {
        FacingRight = !FacingRight;
        transform.Rotate(0, 180, 0);
    }
    private void DetectEdge()
    {
        Vector2 drawLineDir = transform.position + transform.right * SpriteSize;

        bool DetectEdge = Physics2D.Linecast(drawLineDir, drawLineDir + -(Vector2)transform.up, detectEdge);
        Debug.DrawLine(drawLineDir, drawLineDir + -(Vector2)transform.up, Color.red);

        bool DetectWall = Physics2D.Linecast(drawLineDir, drawLineDir + (Vector2)transform.right * WallSensorWidth, detectEdge);
        Debug.DrawLine(drawLineDir, drawLineDir + (Vector2)transform.right * WallSensorWidth, Color.red);


        if (!DetectEdge || DetectWall || Mathf.Abs(enemyRB.velocity.x) < 0.45f)
        {
            EnemyFlip();
        }
    }
    

    private void Tosquash()
    {
        bool UpSensor = Physics2D.Raycast(transform.position, Vector2.up, SquashSensor, UseToSquash);
        bool DownSensor = Physics2D.Raycast(transform.position, Vector2.down, SquashSensor, UseToSquash);

        if(UpSensor && DownSensor)
        {
            Death();
        }
    }
    private void Death()
    {
        //if the enemy dies
        Debug.Log($"Enemy {gameObject.name} Died ");
        gameObject.SetActive(false);
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision != null)
        {
            DetectEdge();
        }
    }


    private void OnDrawGizmosSelected()
    {
        //To draw gizmos
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.up * SquashSensor)); //Squash Sensor in Up Direction
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.down * SquashSensor)); //Squash Sensor in Down Diretion
    }




}


