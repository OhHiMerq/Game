using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private BoxCollider2D thisBoxCollider;
    private PressurePlate_List PpList;

    private bool isActivated = false;
    public bool plateStatus { get { return isActivated; } }

    
    [SerializeField] private LayerMask canPressThis; //choose who can press the plate
    private List<GameObject> objInside; //list of everything inside of the triggerBox

    private void Awake()
    {
        objInside = new List<GameObject>();

        thisBoxCollider = GetComponent<BoxCollider2D>();
        PpList = FindObjectOfType<PressurePlate_List>();
    }
    private void Start()
    {
        PpList.addPressurePlate.Add(gameObject);
    }


    private bool ScanInside(Collider2D collision)
    {
        foreach (GameObject gameObj in objInside)
        {
            if ((canPressThis & 1 << gameObj.gameObject.layer) == 1 << gameObj.gameObject.layer)
            {
                //return true is something is on the plate
                isActivated = true;
                PpList.CheckPlates();
                return isActivated;
            }
        }

        //return false is something is on the plate
        isActivated = false;
        PpList.CheckPlates();
        return isActivated;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        objInside.Add(collision.gameObject);

        ScanInside(collision);

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        objInside.Remove(collision.gameObject);

        ScanInside(collision);

    }
}
