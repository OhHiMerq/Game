using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gravityEffect : MonoBehaviour
{
    
    public static gravityEffect instance;
    //public float rotTime;

    private float targetXRot;
    Vector3 currentRotation;

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
    
    void Start()
    {
        
    }

    public void SwitchGravity(Rigidbody2D objectRB)
    {
        objectRB.gravityScale *= -1; // invert scale
        //StartCoroutine(flipObject(objectRB.transform, objectRB.gravityScale));

        objectRB.transform.Rotate(180, 0, 0);
    }

    /*
    IEnumerator flipObject(Transform objectTransform, float gDir)
    {   

        if (gDir > 0) // down 
        {
            targetXRot = 0;

        }
        else if(gDir < 0) // up
        {
            targetXRot = 180;
        }

        
        float t = 0.0f;
        while (t < rotTime)
        {
            t += Time.deltaTime;
            objectTransform.rotation = Quaternion.Lerp(objectTransform.rotation, Quaternion.Euler(targetXRot, 0, 0), t);
            yield return null;
        }



    }*/
}
