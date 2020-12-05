using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GravityEffect : MonoBehaviour
{
    private Rigidbody2D ThisRB;
    public Rigidbody2D GetThisRB { get { return ThisRB; } }

    private PlayerMove PlayerGravity;
    public PlayerMove GetPlayerGravity { get { return PlayerGravity; } }


    
    protected virtual void Start()
    {
        ThisRB = GetComponent<Rigidbody2D>();
        PlayerGravity = FindObjectOfType<PlayerMove>();


        PlayerGravity.OnGravityEffect += PlayerGravity_OnGravityEffect;
    }
    public void PlayerGravity_OnGravityEffect(object sender, System.EventArgs e)
    {
        InverseGravity();
    }
    public void InverseGravity()
    {
        ThisRB.gravityScale *= -1;
    }

}
