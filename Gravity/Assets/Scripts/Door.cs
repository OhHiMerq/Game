using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IDoor
{
    private Animator DoorAnim;

    [SerializeField] private bool DoorOpen = false;

    private void Start()
    {
        DoorAnim = GetComponentInChildren<Animator>();

    }

    private void Update()
    {
        
    }
    public void OpenDoor()
    {
        //If door is open
        DoorAnim.SetBool("DoorState", true);
    }
    public void CloseDoor()
    {
        //If door is close
        DoorAnim.SetBool("DoorState", false);
    }
    public void Toggle()
    {
        DoorOpen = !DoorOpen;
        if (DoorOpen)
        {
            OpenDoor();
        } else
        {
            CloseDoor();
        }
    }
}
