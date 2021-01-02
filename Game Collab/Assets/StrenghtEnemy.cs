using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrenghtEnemy : Enemy
{
    private bool Squashed = false;
    private bool StopMoving = false;

    private bool InFrontDetect = false;
    [SerializeField] private float InFrontSight = 1f;

    [SerializeField] private float ThrowTimer = 3f;
    private float _throwTimer;
    [SerializeField] private float ThrowForce = 150f;

    public override void Start()
    {
        base.Start();

        _throwTimer = ThrowTimer;
    }
    public override void Update()
    {
        base.Update();

        if (!Squashed)
        {
            ThrowObjectInFront();
        }
        
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();

        
    }

    public override void Movement()
    {
        if (!StopMoving)
        {
            base.Movement();
        }
        else
        {
            MyRigid.velocity = new Vector2(0, MyRigid.velocity.y);
        }
    }

    public override void ImSquashed()
    {
        Squashed = SquashLiftObjDetector() ? true : false;

        if (Squashed)
        {
            StopMoving = true;

            Physics2D.IgnoreLayerCollision(11, 9, true);
            

            if (_throwTimer > 0)
            {
                _throwTimer -= Time.deltaTime;
            }
            else
            {
                ThrowLiftedObject();
            }
        }
        else
        {
            Physics2D.IgnoreLayerCollision(11, 9, false);

            if (!InFrontDetect)
            {
                _throwTimer = ThrowTimer;
                StopMoving = false;
            }
            
        }
    }
    void ThrowLiftedObject()
    {
        Rigidbody2D LiftedObj2 = SquashLiftObjDetector().GetComponent<Rigidbody2D>();

        ThrowObject(LiftedObj2, -transform.right);
    }


    private void ThrowObjectInFront()
    {
        RaycastHit2D HitObj = Physics2D.Raycast(transform.position, transform.right, InFrontSight, CanSquashMe);
        Debug.DrawLine(transform.position, transform.position + (transform.right * InFrontSight), Color.yellow);

        if(HitObj) //If Something Detected inFront
        {
            InFrontDetect = true;
            StopMoving = true;

            //HitObj.collider.transform.position = transform.position; //Hold the box to investigate

            if(_throwTimer > 0)
            {
                _throwTimer -= Time.deltaTime;
            }
            else
            {
                Rigidbody2D _obj = HitObj.collider.gameObject?.GetComponent<Rigidbody2D>();
                ThrowObject(_obj, new Vector2(-transform.right.x, 1));
                Physics2D.IgnoreLayerCollision(11, 8, true);


                StartCoroutine(ResetThrow());
                StopMoving = false;
            }
        }
        else
        {

            StopMoving = false;
            _throwTimer = ThrowTimer;
        }
    }

    IEnumerator ResetThrow()
    {
        yield return new WaitForSeconds(.5f);
        _throwTimer = ThrowTimer;
        Physics2D.IgnoreLayerCollision(11, 8, false);
    }
    

    void ThrowObject(Rigidbody2D rigidbody2D, Vector2 direction)
    {
        rigidbody2D.AddForce(direction * ThrowForce * Time.deltaTime, ForceMode2D.Impulse);
    }

    


    
}
