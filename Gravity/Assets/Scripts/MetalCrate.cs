using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalCrate : Crate
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject.CompareTag("Steel") && collision.gameObject.CompareTag("Magnet"))
        {
            //To Enter the Magnet
            //transform.SetParent(collision.transform);
            GetPlayerGravity.OnGravityEffect -= PlayerGravity_OnGravityEffect;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (gameObject.CompareTag("Steel") && collision.gameObject.CompareTag("Magnet"))
        {
            //To Exit the Magnet
            //transform.SetParent(null);
            GetPlayerGravity.OnGravityEffect += PlayerGravity_OnGravityEffect;
        }
    }
}
