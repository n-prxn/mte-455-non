using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum Gender
{
    male,
    female
}

public class Worker : Unit
{
    private int id;
    public int ID
    {
        get { return id; }
        set { id = value; }
    }

    [SerializeField] private int charSkinID;
    public int CharSkinID
    {
        get { return charSkinID; }
        set { charSkinID = value; }
    }

    public GameObject[] charSkin;

    [SerializeField] private int charFaceID;
    public int CharFaceID
    {
        get { return charFaceID; }
        set { charFaceID = value; }
    }
    public Sprite[] charFacePics;

    [SerializeField] private string staffName;
    public string StaffName
    {
        get { return staffName; }
        set { staffName = value; }
    }

    [SerializeField] private int dailyWage;
    public int DailyWage
    {
        get { return dailyWage; }
        set { dailyWage = value; }
    }

    [SerializeField] private Gender staffGender = Gender.male;
    public Gender StaffGender
    {
        get { return staffGender; }
        set { staffGender = value; }
    }

    [SerializeField] private bool hired = false;
    public bool Hired
    {
        get { return hired; }
        set { hired = value; }
    }
    private int maxAmount = 30;
    [SerializeField] private int curAmount;
    public int CurAmount
    {
        get { return curAmount; }
        set { curAmount = value; }
    }

    [SerializeField] private GameObject targetMine;
    public GameObject TargetMine
    {
        get { return targetMine; }
        set { targetMine = value; }
    }

    [SerializeField] private float miningTimer = 0f;
    [SerializeField] private float miningTimeWait = 1f;
    [SerializeField] private float timeLastDig;
    [SerializeField] private float digRate = 3f;

    protected override void Update()
    {
        base.Update();

        miningTimer += Time.deltaTime;
        if (miningTimer >= miningTimeWait)
        {
            miningTimer = 0f;
            CheckWorkerState();
        }
    }
    public void InitiateCharID(int i)
    {
        charSkinID = i;
        charFaceID = i;
    }

    public void SetGender()
    {
        if (charSkinID == 1 || charSkinID == 4)
        {
            staffGender = Gender.female;
        }
    }

    public void ChangeCharSkin()
    {
        for (int i = 0; i < charSkin.Length; i++)
        {
            if (i == charSkinID)
            {
                charSkin[i].SetActive(true);
            }
            else
            {
                charSkin[i].SetActive(false);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject != targetStructure)
            return;

        Farm farm = other.gameObject.GetComponent<Farm>();

        if ((other.tag == "Farm") && (farm != null) && (farm.HP < 100))
        {
            switch (farm.Stage)
            {
                case FarmStage.plowing:
                    SetUnitState(UnitState.Plow);
                    EquipTools(0);
                    farm.CheckTimeForWork();
                    break;
                case FarmStage.sowing:
                    SetUnitState(UnitState.Sow);
                    EquipTools(1);
                    farm.CheckTimeForWork();
                    break;
                case FarmStage.maintaining:
                    SetUnitState(UnitState.Water);
                    EquipTools(2);
                    farm.CheckTimeForWork();
                    break;
                case FarmStage.harvesting:
                    SetUnitState(UnitState.Harvest);
                    farm.CheckTimeForWork();
                    break;
            }
        }

        Mine mine = other.gameObject.GetComponent<Mine>();
        if ((other.tag == "Mine") && (mine != null) && (mine.HP < 100))
        {
            LookAt(targetMine.transform.position);
            SetUnitState(UnitState.Mining);
        }
    }

    public void HideCharSkin()
    {
        foreach (GameObject obj in charSkin)
        {
            obj.SetActive(false);
        }
    }

    public void DisableAllTools()
    {
        for (int i = 0; i < tools.Length; i++)
            tools[i].SetActive(false);
    }

    private void EquipTools(int i)
    {
        DisableAllTools();
        tools[i].SetActive(true);
    }

    private void CheckWorkerState()
    {
        switch (state)
        {
            case UnitState.MoveToMine:
                MoveToMiningUpdate();
                break;
            case UnitState.Mining:
                MiningUpdate();
                break;
            case UnitState.MoveToDeliver:
                MoveToDeliverUpdate();
                break;
            case UnitState.Deliver:
                DeliverUpdate();
                break;
        }
    }

    #region Resource
    public void StartMining(GameObject mine)
    {
        if (mine == null)
        {
            targetMine = null;
            SetUnitState(UnitState.MoveToDeliver);
            navAgent.SetDestination(targetStructure.transform.position);
        }
        else
        {
            SetUnitState(UnitState.MoveToMine);
            navAgent.SetDestination(mine.transform.position);
        }
        navAgent.isStopped = false;
    }

    void MoveToMiningUpdate()
    {
        if (targetMine == null)
        {
            GameObject newMine = FindingTarget.CheckForNearestMine(targetStructure.transform.position, 100f, "Mine");
            StartMining(newMine);
        }

        EquipTools(3);

        if (Vector3.Distance(transform.position, navAgent.destination) <= 1f)
        {
            LookAt(navAgent.destination);
            SetUnitState(UnitState.Mining);
        }
    }

    void MiningUpdate()
    {
        Mine mine;
        if (targetMine != null)
            mine = targetMine.GetComponent<Mine>();
        else
        {
            GameObject newMine = FindingTarget.CheckForNearestMine(targetStructure.transform.position, 100f, "Mine");
            targetMine = newMine;
            StartMining(newMine);
            return;
        }
        
        EquipTools(3);

        if (Time.time - timeLastDig > digRate)
        {
            timeLastDig = Time.time;
            if (curAmount < maxAmount)
            {
                mine.Deplete(3);
                curAmount += 3;

                if (curAmount > maxAmount)
                    curAmount = maxAmount;
            }
            else
            {
                SetUnitState(UnitState.MoveToDeliver);
                navAgent.SetDestination(targetStructure.transform.position);
                navAgent.isStopped = false;
            }
        }
    }

    private void MoveToDeliverUpdate()
    {
        if (targetStructure == null)
        {
            SetUnitState(UnitState.Idle);
            return;
        }

        DisableAllTools();

        if (Vector3.Distance(transform.position, targetStructure.transform.position) <= 5f)
        {
            SetUnitState(UnitState.Deliver);
            navAgent.isStopped = true;
        }
    }

    private void DeliverUpdate()
    {
        if (targetStructure == null)
        {
            SetUnitState(UnitState.Idle);
            return;
        }

        Office.instance.Stone += curAmount;
        curAmount = 0;
        MainUI.instance.UpdateResourceUi();

        if (targetMine != null)
            StartMining(targetMine);
        else
        {
            GameObject newMine = FindingTarget.CheckForNearestMine(targetStructure.transform.position, 100f, "Mine");
            if (newMine != null)
                StartMining(newMine);
            else
            {
                targetStructure = null;
                SetUnitState(UnitState.Idle);
                navAgent.isStopped = true;
            }
        }
    }
    #endregion
}
