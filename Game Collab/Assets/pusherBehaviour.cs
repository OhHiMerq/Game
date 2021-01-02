using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pusherBehaviour : MonoBehaviour
{
    public float pushStrength;
    public float pushEffectTime;
    public LayerMask toPushLayers;
    public PressurePlate plate;
    private List<GameObject> triggerObj = new List<GameObject>();
    private Dictionary<GameObject, bool> pushCoroutineObj = new Dictionary<GameObject, bool>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (plate.plateStatus)
        {
            for (int i = 0; i < triggerObj.Count; i++)
            {
                if (!pushCoroutineObj[triggerObj[i]])
                {
                    StartCoroutine(PushObject(pushEffectTime, triggerObj[i].GetComponent<Rigidbody2D>(), i));
                }

            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Contains(toPushLayers,collision.gameObject.layer))
        {
            if (!triggerObj.Contains(collision.gameObject))
            {
                triggerObj.Add(collision.gameObject);
                //Debug.Log("Key Added: " + triggerObj.IndexOf(collision.gameObject));
                pushCoroutineObj.Add(collision.gameObject, false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (Contains(toPushLayers, collision.gameObject.layer))
        {
            if (triggerObj.Contains(collision.gameObject))
            {
                pushCoroutineObj.Remove(collision.gameObject);
                //Debug.Log("Key Removed: " + triggerObj.IndexOf(collision.gameObject));
                triggerObj.Remove(collision.gameObject);
            }
        }
    }


    public static bool Contains(LayerMask mask, int layer)
    {
        return ((mask & (1 << layer)) != 0);
    }
    


    IEnumerator PushObject(float timeLeft,Rigidbody2D rbToPush,int i)
    {
        pushCoroutineObj[triggerObj[i]] = true;
        //Debug.Log("StartCoroutine");
        GetComponent<Animator>().Play("pushAnim");

        DisableVelocityEffectScripts(rbToPush.transform);

        while (timeLeft >= 0.0f)
        {
            
            timeLeft -= Time.deltaTime;
            rbToPush.AddForce(transform.right * pushStrength * Time.deltaTime, ForceMode2D.Impulse);
            yield return null;  

        }

        DisableVelocityEffectScripts(rbToPush.transform);

        
        if (triggerObj.Count > 0 && triggerObj[i] != null)  // dadaan dito pag yung object ay kahit na push na ay nasa harap parin ng pusher. then mag rurun ulit yung coroutine sa kanya
        {
            pushCoroutineObj[triggerObj[i]] = false;
        }
        

    }

    void DisableVelocityEffectScripts(Transform obj)
    {
        if (obj.GetComponent<playerMovement>()) // if meron 
        {
            //Debug.Log("Velocity script");
            obj.GetComponent<playerMovement>().enabled = !obj.GetComponent<playerMovement>().enabled;
        }
    }
}
