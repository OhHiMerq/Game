using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pusherBehaviour : MonoBehaviour
{
    public float pushStrength;
    public float pushEffectTime;
    public LayerMask toPushLayers;
    public float detectionLength;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
      /*
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, detectionLength, toPushLayers);
        Debug.DrawLine(transform.position, transform.position + (transform.right * detectionLength), Color.green);
        if (hit.collider != null)
        {
            Debug.Log(hit.transform.name);
            //hit.transform.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.right * pushStrength,ForceMode2D.Impulse);
            StartCoroutine(PushObject(pushEffectTime, hit.transform.gameObject.GetComponent<Rigidbody2D>()));
        }*/
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Contains(toPushLayers,collision.gameObject.layer))
        {
            //collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.right * pushStrength, ForceMode2D.Impulse);
            GetComponent<Animator>().Play("pushAnim");
            StartCoroutine(PushObject(pushEffectTime, collision.gameObject.GetComponent<Rigidbody2D>()));
        }
    }

    public static bool Contains(LayerMask mask, int layer)
    {
        return ((mask & (1 << layer)) != 0);
    }
    


    IEnumerator PushObject(float timeLeft,Rigidbody2D rbToPush)
    {
        Debug.Log("StartCoroutine");

        DisableVelocityEffectScripts(rbToPush.transform);

        while (timeLeft >= 0.0f)
        {
            
            timeLeft -= Time.deltaTime;
            rbToPush.AddForce(transform.right * pushStrength, ForceMode2D.Impulse);
            yield return null;  

        }

        DisableVelocityEffectScripts(rbToPush.transform);
    }

    void DisableVelocityEffectScripts(Transform obj)
    {
        if (obj.GetComponent<playerMovement>()) // if meron 
        {
            Debug.Log("Velocity script");
            obj.GetComponent<playerMovement>().enabled = !obj.GetComponent<playerMovement>().enabled;
        }
    }
}
