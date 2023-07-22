using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class StructureManager : MonoBehaviour
{
    [SerializeField] private bool isConstructing;
    [SerializeField] private bool isDemolishing;

    [SerializeField] private GameObject curBuildingPrefab;
    [SerializeField] private GameObject buildingParent;

    [SerializeField] private Vector3 curCursorPos;

    public GameObject buildingCursor;
    public GameObject gridPlane;

    private GameObject ghostBuilding;

    [SerializeField] private GameObject _curStructure;

    public GameObject CurStructure{
        get {return _curStructure;}
        set {_curStructure = value;}
    }

    [SerializeField] private GameObject[] structurePrefab;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            CancelStructureMode();

        curCursorPos = Formula.instance.GetCurTilePosition();
        if(isConstructing){
            buildingCursor.transform.position = curCursorPos;
            gridPlane.SetActive(true);
        }else{
            gridPlane.SetActive(false);
        }

        CheckLeftClick();
        CheckRightClick();
    }

    public void BeginNewBuildingPlacement(GameObject prefab){
        if(CheckMoney(prefab) == false)
            return;

        isDemolishing = false;
        isConstructing = true;

        curBuildingPrefab = prefab;

        ghostBuilding = Instantiate(curBuildingPrefab, curCursorPos, Quaternion.identity);
        ghostBuilding.GetComponent<FindBuildingSite>().Plane.SetActive(true);

        buildingCursor = ghostBuilding;
        buildingCursor.SetActive(true);
    }

    public void PlaceBuilding(){
        if(!buildingCursor.GetComponent<FindBuildingSite>().CanBuild)
            return;
        
        GameObject structureObj = Instantiate(curBuildingPrefab, curCursorPos, ghostBuilding.transform.rotation, buildingParent.transform);

        Structure s = structureObj.GetComponent<Structure>();
        Office.instance.AddBuilding(s);

        DeductMoney(s.CostToBuild);
        if(!CheckMoney(structureObj))
            CancelStructureMode();
    }

    private void CheckLeftClick(){
        if(Input.GetMouseButtonDown(0)){
            if(isConstructing)
                PlaceBuilding();
            else
                CheckOpenPanel();
        }
    }

    private void CancelStructureMode(){
        isConstructing = false;
        if(buildingCursor != null)
            buildingCursor.SetActive(false);

        if(ghostBuilding != null)
            Destroy(ghostBuilding);
    }

    private void RotateBuilding(){
        ghostBuilding.transform.rotation *= Quaternion.Euler(Vector3.up * 90);
    }

    private void CheckRightClick(){
        if(Input.GetMouseButtonDown(1)){
            RotateBuilding();
        }
    }

    private bool CheckMoney(GameObject obj){
        int cost = obj.GetComponent<Structure>().CostToBuild;
        if(cost <= Office.instance.Money)
            return true;
        else   
            return false;
    }

    private void DeductMoney(int cost){
        Office.instance.Money -= cost;
        MainUI.instance.UpdateResourceUi();
    }

    public void OpenFarmPanel(){
        string name = CurStructure.GetComponent<Farm>().StructureName;

        MainUI.instance.FarmNameText.text = name;
        MainUI.instance.ToggleFarmPanel();
    }

    private void CheckOpenPanel(){
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 1000)){
            if(EventSystem.current.IsPointerOverGameObject())
                return;
            
            CurStructure = hit.collider.gameObject;
            switch(hit.collider.tag){
                case "Farm":
                    OpenFarmPanel();
                    break;
            }
        }
    }

    public void CallStaff(){
        Office.instance.SendStaff(CurStructure);
        MainUI.instance.UpdateResourceUi();
    }
}
