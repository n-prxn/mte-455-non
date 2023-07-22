using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Office : MonoBehaviour
{
    [SerializeField] private int money;
    public int Money{
        get{return money;}
        set{money = value;}
    }

    [SerializeField] private List<Worker> workers = new List<Worker>();
    public List<Worker> Workers{
        get{return workers;}
        set{workers = value;}
    }

    [SerializeField] private int wheat;
    public int Wheat{
        get{return wheat;}
        set{wheat = value;}
    }

    [SerializeField] private int melon;
    public int Melon{
        get{return melon;}
        set{melon = value;}
    }

    [SerializeField] int corn;
    public int Corn{
        get{return corn;}
        set{corn = value;}
    }

    [SerializeField] int milk;
    public int Milk{
        get{return milk;}
        set{milk = value;}
    }

    [SerializeField] int apple;
    public int Apple{
        get{return apple;}
        set{apple = value;}
    }

    [SerializeField] private int dailyCostWages;
    [SerializeField] private List<Structure> structures = new List<Structure>();
    public List<Structure> Structures{
        get{return structures;}
        set{structures = value;}
    }

    [SerializeField] private int availStaff;
    public int AvailStaff{
        get{return availStaff;}
        set{availStaff = value;}
    }

    [SerializeField] private GameObject staffParent;
    [SerializeField] private GameObject spawnPosition;
    [SerializeField] private GameObject rallyPosition;

    public static Office instance;
    void Awake(){
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

    public void AddBuilding(Structure s){
        structures.Add(s);
    }

    public void RemoveBuilding(Structure s){
        structures.Remove(s);
        Destroy(s.gameObject);
    }

    public bool ToHireStaff(GameObject workerObj){
        if(money <= 0)
            return false;
        
        workerObj.transform.parent = staffParent.transform;
        Worker w = workerObj.GetComponent<Worker>();
        w.Hired = true;
        w.ChangeCharSkin();
        w.SetToWalk(rallyPosition.transform.position);
        
        money -= w.DailyWage;
        AddStaff(w);

        MainUI.instance.UpdateResourceUi();

        return true;
    }

    public void AddStaff(Worker w){
        workers.Add(w);
        dailyCostWages += w.DailyWage;
    }

    public void UpdateAvailStaff(){
        availStaff = 0;
        foreach(Worker w in workers){
            if(w.TargetStructure == null)
                availStaff++;
        }
    }

    public void SendStaff(GameObject target){
        Farm f = target.GetComponent<Farm>();
        
        int staffNeed = f.MaxStaffNum - f.CurrentWorkers.Count;
        if(staffNeed <= 0)
            return;

        UpdateAvailStaff();

        if(staffNeed > availStaff)
            staffNeed = availStaff;

        int n = 0;
        for(int i = 0; i < workers.Count ; i++){
            if(workers[i].TargetStructure == null){
                Worker w = workers[i].GetComponent<Worker>();
                workers[i].TargetStructure = target;
                workers[i].SetToWalk(target.transform.position);
                f.AddStaffToFarm(w);
                n++;
            }

            if(n >= staffNeed)
                break;
        }

        UpdateAvailStaff();
    }
}
