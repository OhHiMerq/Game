using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheDoorToOpen
{
    GameObject TheDoor;

    public TheDoorToOpen(GameObject Door)
    {
        TheDoor = Door;
    }

    public void ToOpen()
    {
        TheDoor.GetComponent<IDoor>().OpenDoor();
    }
    public void ToClose()
    {
        TheDoor.GetComponent<IDoor>().CloseDoor();
    }
    public void ToToggle()
    {
        TheDoor.GetComponent<IDoor>().Toggle();
    }
}
