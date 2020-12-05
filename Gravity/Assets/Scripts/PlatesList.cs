using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesList : MonoBehaviour
{
    private List<GameObject> ListOfPlates = new List<GameObject>();
    public List<GameObject> GetListOfPlates { get { return ListOfPlates; } }

    [SerializeField] private GameObject MainDoor;

    TheDoorToOpen openTheDoor;
    private void Awake()
    {
        openTheDoor = new TheDoorToOpen(MainDoor);   
    }


    private bool ScanAllPlates()
    {
        foreach (GameObject Plates in ListOfPlates)
        {
            PressurePlate _plate = Plates.GetComponent<PressurePlate>();

            if (!_plate.GetActivateInfo)
            {
                return false;
            }
        }

        //if all plates is activated
        return true;
    }
    public void CheckAllPlates()
    {
        if (ScanAllPlates())
        {
            openTheDoor.ToOpen();
        }
        else
        {
            openTheDoor.ToClose();
        }
    }

    
}
