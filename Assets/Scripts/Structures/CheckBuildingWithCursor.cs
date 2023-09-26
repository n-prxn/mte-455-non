using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckBuildingWithCursor : MonoBehaviour
{
    private GameObject currentBuilding = null;
    private void OnTriggerEnter(Collider other)
    {
        currentBuilding = other.gameObject;
        if (currentBuilding.layer == LayerMask.NameToLayer("Building"))
        {
            //FindBuildingSite buildingSite = currentBuilding.get
        }
    }

}
