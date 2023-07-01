using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FarmStage{
    plowing,
    sowing,
    maintaining,
    harvesting
}

public class Farm : Structure
{
    [SerializeField] private FarmStage stage = FarmStage.plowing;
    [SerializeField] private int maxStaffNum = 3;

    public int MaxStaffNum{
        get {return maxStaffNum;}
        set {maxStaffNum = value;}
    }

    [SerializeField] private int dayRequired;
    [SerializeField] private int dayPassed;

    [SerializeField] private float produceTimer = 0f;
    private int secondsPerDay = 10;

    [SerializeField] private GameObject FarmUI;
    
    [SerializeField] private List<Worker> currentWorkers;
    public List<Worker> CurrentWorkers{
        get {return currentWorkers;}
        set {currentWorkers = value;}
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlowing();
        CheckSowing();
        CheckMaintaining();
        CheckHarvesting();
    }

    public void CheckPlowing(){
        if((hp >= 100) && (stage == FarmStage.plowing)){
            stage = FarmStage.sowing;
            hp = 1;
        }
    }

    public void CheckSowing(){
        if((hp >= 100) && (stage == FarmStage.sowing)){
            functional = true;
            stage = FarmStage.maintaining;
            hp = 1;
        }
    }

    public void CheckMaintaining(){
        if((hp >= 100) && (stage == FarmStage.sowing)){
            produceTimer += Time.deltaTime;
            dayPassed = Mathf.CeilToInt(produceTimer / secondsPerDay);
        }
        
        if((functional == true) && (dayPassed >= dayRequired)){
            produceTimer = 0;
            stage = FarmStage.harvesting;
            hp = 1;
        }
    }

    public void CheckHarvesting(){
        if((hp >= 100) && (stage == FarmStage.harvesting)){
            Debug.Log("Harvest +1000");
            hp = 1;
            stage = FarmStage.sowing;
        }
    }
}
