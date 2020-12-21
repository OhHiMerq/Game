using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchingGravityEffect : MonoBehaviour
{
    public static SwitchingGravityEffect instance;


    private void Awake() // singleton
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public void Switch(Rigidbody2D objectRB, bool ToRotate)
    {
        objectRB.gravityScale *= -1; // invert scale

        if (ToRotate)
        {
            objectRB.transform.Rotate(180, 0, 0);
        }
    }
}
