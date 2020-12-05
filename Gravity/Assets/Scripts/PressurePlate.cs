using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private BoxCollider2D ThisBoxCollider;
    private PlatesList platesList;

    private bool ActiveStatus = false;
    public bool GetActivateInfo { get { return ActiveStatus; } }

    [SerializeField] private bool ToOpenOtherDoor;
    [SerializeField] private GameObject TheDoorToOpen;
    

    public LayerMask DetectPressed;
    TheDoorToOpen DoorStatus;

    private void Awake()
    {
        //platesList = FindObjectOfType<PlatesList>();
        platesList = transform.parent.GetComponent<PlatesList>();

        ThisBoxCollider = GetComponent<BoxCollider2D>();

        DoorStatus = new TheDoorToOpen(TheDoorToOpen);
    }
    private void Start()
    {
        if (!ToOpenOtherDoor)
        {
            platesList.GetListOfPlates.Add(gameObject);
        }
        
    }


    private void ToActivateMainDoor(Collider2D collider, bool Status)
    {
        bool activateTheDoor = Physics2D.OverlapBox((Vector2)transform.position + ThisBoxCollider.offset,
                transform.localScale * ThisBoxCollider.size, 1, DetectPressed);

        if (activateTheDoor)
        {
            ActiveStatus = Status;
            platesList.CheckAllPlates();
            return;
        }

        ActiveStatus = false;
        platesList.CheckAllPlates();
    }
    private void ToActivateOtherDoor()
    {
        bool activateTheDoor = Physics2D.OverlapBox((Vector2)transform.position + ThisBoxCollider.offset,
            transform.localScale * ThisBoxCollider.size, 1, DetectPressed);

        if (activateTheDoor)
        {
            DoorStatus.ToOpen();
        }
        else
        {
            DoorStatus.ToClose();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Holding
        
        if (ToOpenOtherDoor)
        {
            if((DetectPressed & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
            {
                DoorStatus.ToOpen();
            }
            //ToActivateOtherDoor();
        }
        else
        {
            if ((DetectPressed & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
            {
                ToActivateMainDoor(collision, true);
            }
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //Not holding

        if (ToOpenOtherDoor)
        {
            if ((DetectPressed & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
            {
                DoorStatus.ToClose();
            }
            //ToActivateOtherDoor();
        }
        else
        {
            if ((DetectPressed & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
            {
                ToActivateMainDoor(collision, false);
            }
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2)transform.position + ThisBoxCollider.offset, transform.localScale * ThisBoxCollider.size);
    }
}
