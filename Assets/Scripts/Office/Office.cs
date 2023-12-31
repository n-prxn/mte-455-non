using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Office : MonoBehaviour
{
    [SerializeField] private int money;
    public int Money
    {
        get { return money; }
        set { money = value; }
    }

    [SerializeField] private List<Worker> workers = new List<Worker>();
    public List<Worker> Workers
    {
        get { return workers; }
        set { workers = value; }
    }

    [SerializeField] private int wheat;
    public int Wheat
    {
        get { return wheat; }
        set { wheat = value; }
    }

    [SerializeField] private int melon;
    public int Melon
    {
        get { return melon; }
        set { melon = value; }
    }

    [SerializeField] int corn;
    public int Corn
    {
        get { return corn; }
        set { corn = value; }
    }

    [SerializeField] int milk;
    public int Milk
    {
        get { return milk; }
        set { milk = value; }
    }

    [SerializeField] int apple;
    public int Apple
    {
        get { return apple; }
        set { apple = value; }
    }

    [SerializeField] private int stone;
    public int Stone
    {
        get { return stone; }
        set { stone = value; }
    }
    [SerializeField] private int wood;
    public int Wood
    {
        get { return wood; }
        set { wood = value; }
    }

    [SerializeField] private int dailyCostWages;
    [SerializeField] private List<Structure> structures = new List<Structure>();
    public List<Structure> Structures
    {
        get { return structures; }
        set { structures = value; }
    }

    [SerializeField] private int availStaff;
    public int AvailStaff
    {
        get { return availStaff; }
        set { availStaff = value; }
    }

    [SerializeField] private GameObject staffParent;
    [SerializeField] private GameObject spawnPosition;
    public GameObject SpawnPosition
    {
        get { return spawnPosition; }
    }
    [SerializeField] private GameObject rallyPosition;

    public static Office instance;

    [Header("Building")]
    [SerializeField] private int unitLimit = 3;
    [SerializeField] private int housingUnitNum = 6;
    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddBuilding(Structure s)
    {
        structures.Add(s);
        CheckHousing();
    }

    public void RemoveBuilding(Structure s)
    {
        structures.Remove(s);
        Destroy(s.gameObject);
        CheckHousing();
    }

    public bool ToHireStaff(GameObject workerObj)
    {
        if (money <= 0)
            return false;

        if (workers.Count >= unitLimit)
            return false;

        workerObj.transform.parent = staffParent.transform;
        Worker w = workerObj.GetComponent<Worker>();

        w.Hired = true;

        if (FindingTarget.CheckForNearestHouse(spawnPosition.transform.position, 500f, LayerMask.GetMask("Building")) != null)
        {
            rallyPosition.transform.position = FindingTarget.CheckForNearestHouse(spawnPosition.transform.position, 500f, LayerMask.GetMask("Building")).transform.GetChild(2).transform.position;
        }

        w.SetToWalk(rallyPosition.transform.position);

        money -= w.DailyWage;
        AddStaff(w);

        MainUI.instance.UpdateResourceUi();

        return true;
    }

    public void AddStaff(Worker w)
    {
        workers.Add(w);
        dailyCostWages += w.DailyWage;
    }

    public void UpdateAvailStaff()
    {
        availStaff = 0;
        foreach (Worker w in workers)
        {
            if (w.TargetStructure == null)
                availStaff++;
        }
    }

    public void SendStaff(GameObject target)
    {
        Farm f = target.GetComponent<Farm>();

        int staffNeed = f.MaxStaffNum - f.CurrentWorkers.Count;
        if (staffNeed <= 0)
            return;

        UpdateAvailStaff();

        if (staffNeed > availStaff)
            staffNeed = availStaff;

        int n = 0;
        for (int i = 0; i < workers.Count; i++)
        {
            if (workers[i].TargetStructure == null)
            {
                Worker w = workers[i].GetComponent<Worker>();
                workers[i].TargetStructure = target;
                workers[i].SetToWalk(target.transform.position);
                f.AddStaffToFarm(w);
                n++;
            }

            if (n >= staffNeed)
                break;
        }

        UpdateAvailStaff();
    }

    public void FireStaff(Worker w)
    {
        workers.Remove(w);
        dailyCostWages -= w.DailyWage;
    }

    public bool ToFireStaff(GameObject staffObj)
    {
        staffObj.transform.parent = LaborMarket.instance.WorkerParent.transform;

        Worker w = staffObj.GetComponent<Worker>();

        w.Hired = false;

        if (w.TargetStructure != null)
        {
            Farm f = w.TargetStructure.GetComponent<Farm>();
            if (f != null)
                f.CurrentWorkers.Remove(w);
        }

        w.TargetStructure = null;
        w.SetToWalk(spawnPosition.transform.position);
        w.DisableAllTools();

        FireStaff(w);
        MainUI.instance.UpdateResourceUi();

        return true;
    }

    public void CheckHousing()
    {
        unitLimit = 3;

        foreach (Structure s in structures)
        {
            //if (true)
            unitLimit += housingUnitNum;
        }

        if (unitLimit >= 100)
            unitLimit = 100;
        else if (unitLimit < 0)
            unitLimit = 0;
    }

    public void SendWorkerToMine(GameObject mine, GameObject warehouse, int workerAmount = 1)
    {
        UpdateAvailStaff();

        if (mine == null || availStaff <= 0)
            return;

        int n = 0;

        for (int i = 0; i < workers.Count; i++)
        {
            if (workers[i].TargetStructure == null)
            {
                Worker w = workers[i].GetComponent<Worker>();
                workers[i].TargetStructure = warehouse;
                workers[i].TargetResource = mine;
                w.StartCollectResource(mine);
                n++;
            }

            if (n >= workerAmount)
                break;
        }

        UpdateAvailStaff();
    }
}
