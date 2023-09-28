using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckBuildingWithCursor : MonoBehaviour
{
    [SerializeField] private StructureManager structureManager;
    private GameObject currentBuilding = null;
    private void OnTriggerEnter(Collider other)
    {
        currentBuilding = other.gameObject;

        if (!structureManager.IsDemolishing)
            return;

        if (currentBuilding.layer == LayerMask.NameToLayer("Building"))
        {
            FindBuildingSite buildingSite = currentBuilding.GetComponent<FindBuildingSite>();
            buildingSite.Plane.SetActive(true);
            buildingSite.ChangeColor(Color.red);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        currentBuilding = other.gameObject;

        if (!structureManager.IsDemolishing)
            return;
            
        if (currentBuilding.layer == LayerMask.NameToLayer("Building"))
        {
            FindBuildingSite buildingSite = currentBuilding.GetComponent<FindBuildingSite>();
            buildingSite.ChangeColor(Color.white);
            buildingSite.Plane.SetActive(false);

        }
    }

}
