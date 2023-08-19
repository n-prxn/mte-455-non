using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
public class StructureManager : MonoBehaviour
{
    [SerializeField] private bool isConstructing;
    [SerializeField] private bool isDemolishing;

    [SerializeField] private GameObject curBuildingPrefab;
    [SerializeField] private GameObject buildingParent;

    [SerializeField] private Vector3 curCursorPos;

    public GameObject buildingCursor, demolishCursor;
    public GameObject gridPlane;

    private GameObject ghostBuilding;

    [SerializeField] private GameObject _curStructure;

    public GameObject CurStructure
    {
        get { return _curStructure; }
        set { _curStructure = value; }
    }

    [SerializeField] private GameObject[] structurePrefab;
    private Camera cam;

    [Header("Road")]
    [SerializeField] private GameObject roadGreenTilePrefab;
    [SerializeField] private GameObject roadGreenTileParent;
    [SerializeField] private Vector3 startRoadPos;
    [SerializeField] private Vector3 endRoadPos;
    [SerializeField] private List<GameObject> roadGreenTilesList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CancelStructureMode();
            CancelDemolishingMode();
        }

        curCursorPos = Formula.instance.GetCurTilePosition();
        if (isConstructing)
        {
            buildingCursor.transform.position = curCursorPos;
            CheckRightClick();
            gridPlane.SetActive(true);
        }
        else if (isDemolishing)
        {
            demolishCursor.transform.position = curCursorPos;
            gridPlane.SetActive(true);
        }
        else
        {
            gridPlane.SetActive(false);
        }

        CheckLeftClick();
        CheckRoadMode();

        //CursorAreaCheck.instance.Cursor = curCursorPos;
    }

    public void BeginNewBuildingPlacement(GameObject prefab)
    {
        if (CheckMoney(prefab) == false)
            return;

        isDemolishing = false;
        isConstructing = true;

        curBuildingPrefab = prefab;

        ghostBuilding = Instantiate(curBuildingPrefab, curCursorPos, Quaternion.identity);
        ghostBuilding.GetComponent<FindBuildingSite>().Plane.SetActive(true);

        buildingCursor = ghostBuilding;
        buildingCursor.SetActive(true);
    }

    public void PlaceBuilding()
    {
        if (!buildingCursor.GetComponent<FindBuildingSite>().CanBuild)
            return;

        GameObject structureObj = Instantiate(curBuildingPrefab, curCursorPos, ghostBuilding.transform.rotation, buildingParent.transform);

        Structure s = structureObj.GetComponent<Structure>();
        Office.instance.AddBuilding(s);

        DeductMoney(s.CostToBuild);
        if (!CheckMoney(structureObj))
            CancelStructureMode();
    }

    private void CheckLeftClick()
    {
        if (buildingCursor != null && buildingCursor.tag == "Road")
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (isConstructing)
                PlaceBuilding();
            else if (isDemolishing)
                Demolish();
            else
                CheckOpenPanel();
        }
    }

    private void CancelStructureMode()
    {
        isConstructing = false;
        if (buildingCursor != null)
            buildingCursor.SetActive(false);

        if (ghostBuilding != null)
            Destroy(ghostBuilding);
    }

    private void CancelDemolishingMode()
    {
        isDemolishing = false;
        if (demolishCursor != null)
            demolishCursor.SetActive(false);
    }

    private void RotateBuilding()
    {
        ghostBuilding.transform.rotation *= Quaternion.Euler(Vector3.up * 90);
    }

    private void CheckRightClick()
    {
        if (Input.GetMouseButtonDown(1) && buildingCursor.tag != "Road")
        {
            RotateBuilding();
        }
    }

    private bool CheckMoney(GameObject obj)
    {
        int cost = obj.GetComponent<Structure>().CostToBuild;
        if (cost <= Office.instance.Money)
            return true;
        else
            return false;
    }

    private void DeductMoney(int cost)
    {
        Office.instance.Money -= cost;
        MainUI.instance.UpdateResourceUi();
    }

    public void OpenFarmPanel()
    {
        string name = CurStructure.GetComponent<Farm>().StructureName;

        MainUI.instance.FarmNameText.text = name;
        MainUI.instance.ToggleFarmPanel();
    }

    private void CheckOpenPanel()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            CurStructure = hit.collider.gameObject;
            switch (hit.collider.tag)
            {
                case "Farm":
                    {
                        OpenFarmPanel();
                        break;
                    }
            }
        }
    }

    public void CallStaff()
    {
        Office.instance.SendStaff(CurStructure);
        MainUI.instance.UpdateResourceUi();
    }

    private void Demolish()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            switch (hit.collider.tag)
            {
                case "Farm":
                    {
                        Office.instance.RemoveBuilding(hit.collider.GetComponent<Structure>());
                        break;
                    }
            }
        }

        /*Structure structure = Office.instance.Structures.Find(x => x.transform.position == curCursorPos);

        if(structure != null){
            Office.instance.RemoveBuilding(structure);
        }*/

        MainUI.instance.UpdateResourceUi();
    }

    public void ToggleDemolish()
    {
        isConstructing = false;
        isDemolishing = !isDemolishing;

        gridPlane.SetActive(isDemolishing);
        demolishCursor.SetActive(isDemolishing);
    }

    #region Road
    private void GenGreenTile(Vector3 pos)
    {
        GameObject greenTileObj = Instantiate(roadGreenTilePrefab, pos, Quaternion.identity, roadGreenTileParent.transform);
        roadGreenTilesList.Add(greenTileObj);
    }

    private void ClearGreenTileList()
    {
        foreach (GameObject tileObj in roadGreenTilesList)
            Destroy(tileObj);
        roadGreenTilesList.Clear();
    }

    private List<Vector3> FindNewPath(Vector3 startPos, Vector3 endPos)
    {
        List<Vector3> path = new List<Vector3>();
        int xTileNum = ((int)endRoadPos.x - (int)startRoadPos.x) / 5;
        int zTileNum = ((int)endRoadPos.z - (int)startRoadPos.z) / 5;

        int xModifier = xTileNum >= 0 ? 1 : -1;
        int zModifier = zTileNum >= 0 ? 1 : -1;

        path.Add(startPos);

        for (int i = 1; i <= Mathf.Abs(xTileNum); i++)
        {
            path.Add(startPos + new Vector3(5f * i * xModifier, 0f, 0f));
        }

        Vector3 corner = path.Count > 1 ? path[path.Count - 1] : startRoadPos;

        for (int i = 1; i <= Mathf.Abs(zTileNum); i++)
        {
            path.Add(corner + new Vector3(0f, 0f, 5f * i * zModifier));
        }

        return path;
    }

    private void PlanningRoad()
    {
        curCursorPos = Formula.instance.GetCurTilePosition();

        if (endRoadPos == curCursorPos)
            return;

        endRoadPos = curCursorPos;

        List<Vector3> newPath = FindNewPath(startRoadPos, endRoadPos);
        ClearGreenTileList();

        foreach (Vector3 pos in newPath)
        {
            GenGreenTile(pos);
        }
    }

    private void ConstructRoad()
    {
        foreach (GameObject tileObj in roadGreenTilesList)
        {
            if (CheckMoney(curBuildingPrefab) == false)
                CancelStructureMode();
            else
            {
                GameObject roadObj = Instantiate(curBuildingPrefab, tileObj.transform.position, Quaternion.identity, buildingParent.transform);
                Structure s = roadObj.GetComponent<Structure>();
                Office.instance.AddBuilding(s);
                DeductMoney(s.CostToBuild);
            }
        }
        ClearGreenTileList();
        CancelStructureMode();
    }

    private void CheckRoadMode()
    {
        if (buildingCursor == null || buildingCursor.tag != "Road")
            return;

        if (buildingCursor.GetComponent<FindBuildingSite>().CanBuild == false)
            return;

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(0))
        {
            isConstructing = true;
            startRoadPos = curCursorPos;
            roadGreenTilesList.Clear();

            GameObject greenTileObj = Instantiate(roadGreenTileParent, startRoadPos, Quaternion.identity, roadGreenTileParent.transform);
            roadGreenTilesList.Add(greenTileObj);
        }

        if (Input.GetMouseButton(0))
            PlanningRoad();

        if (Input.GetMouseButtonUp(0))
            ConstructRoad();
    }
    #endregion
}
