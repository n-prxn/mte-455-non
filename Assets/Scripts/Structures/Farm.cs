using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FarmStage
{
    plowing,
    sowing,
    maintaining,
    harvesting
}

public class Farm : Structure
{
    [SerializeField] private FarmStage stage = FarmStage.plowing;
    public FarmStage Stage
    {
        get { return stage; }
    }
    [SerializeField] private int maxStaffNum = 3;

    public int MaxStaffNum
    {
        get { return maxStaffNum; }
        set { maxStaffNum = value; }
    }

    [SerializeField] private int dayRequired;
    [SerializeField] private int dayPassed;

    [SerializeField] private float produceTimer = 0f;
    private int secondsPerDay = 10;

    private float WorkTimer = 0f;
    private float WorkTimeWait = 1f;

    [SerializeField] private GameObject FarmUI;

    [SerializeField] private List<Worker> currentWorkers;
    public List<Worker> CurrentWorkers
    {
        get { return currentWorkers; }
        set { currentWorkers = value; }
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

    public void CheckPlowing()
    {
        if ((hp >= 100) && (stage == FarmStage.plowing))
        {
            stage = FarmStage.sowing;
            hp = 1;
        }
    }

    public void CheckSowing()
    {
        if ((hp >= 100) && (stage == FarmStage.sowing))
        {
            functional = true;
            stage = FarmStage.maintaining;
            hp = 1;
        }
    }

    public void CheckMaintaining()
    {
        if ((hp >= 100) && (stage == FarmStage.maintaining))
        {
            produceTimer += Time.deltaTime;
            dayPassed = Mathf.CeilToInt(produceTimer / secondsPerDay);

            if ((functional == true) && (dayPassed >= dayRequired))
            {
                produceTimer = 0;
                stage = FarmStage.harvesting;
                hp = 1;
            }
        }
    }

    public void CheckHarvesting()
    {
        if ((hp >= 100) && (stage == FarmStage.harvesting))
        {
            Debug.Log("Harvest +1000");
            HarvestResult();
            hp = 1;
            stage = FarmStage.sowing;
        }
    }

    public void AddStaffToFarm(Worker w)
    {
        currentWorkers.Add(w);
    }

    private void Working()
    {
        hp += 3;
    }

    public void CheckTimeForWork()
    {
        WorkTimer += Time.deltaTime;
        if (WorkTimer >= WorkTimeWait)
        {
            WorkTimer = 0;
            Working();
        }
    }

    public void HarvestResult()
    {
        switch (structureType)
        {
            case StructureType.wheat:
                Office.instance.Wheat += 1000;
                break;
            case StructureType.apple:
                Office.instance.Apple += 1000;
                break;
        }

        MainUI.instance.UpdateResourceUi();
    }
}
