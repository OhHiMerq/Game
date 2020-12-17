using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate_List : MonoBehaviour
{
    private List<GameObject> pressurePlatesList;
    public List<GameObject> addPressurePlate { get { return pressurePlatesList; } }

    [SerializeField] private Door _Door;

    private void Awake()
    {
        pressurePlatesList = new List<GameObject>();
    }

    private bool ScanPlatesActivated()
    {
        foreach (GameObject _pressurePlate in pressurePlatesList)
        {
            PressurePlate Pp = _pressurePlate.GetComponent<PressurePlate>();

            if (!Pp.plateStatus)
            {
                return false; //if one plate still not activated, it will return false
            }
        }

        return true; //if the plates are all activated, it will return true
    }
    public void CheckPlates()
    {
        if (ScanPlatesActivated())
        {
            //Open the Door
            _Door.OpenDoor();
        }
        else
        {
            //Close the Door
            _Door.CloseDoor();
        }
    }
}
