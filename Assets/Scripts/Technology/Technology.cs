using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public struct TechCost
{
    public int money;
    public int stone;
    public int wood;
}

public enum TechState
{
    Locked,
    Unlocked,
    InProgress,
    Completed
}

[System.Serializable]
public class Technology
{
    [SerializeField] private int id;
    public int ID { get => id; set => id = value; }

    [SerializeField] private string techName;
    public string TechName { get => techName; set => techName = value; }

    [SerializeField] private Sprite icon;
    public Sprite Icon { get => icon; set => icon = value; }

    [SerializeField] private List<int> requiredTechID = new List<int>();
    public List<int> RequiredTechID { get => requiredTechID; set => requiredTechID = value; }

    [SerializeField] private TechCost cost;
    public TechCost Cost { get => cost; set => cost = value; }

    [SerializeField] private int daysRequired;
    public int DaysRequired { get => daysRequired; set => daysRequired = value; }

    [SerializeField] private string description;
    public string Description { get => description; set => description = value; }

    [SerializeField] private TechState state;
    public TechState State { get => state; set => state = value; }

    public void InitData(TechSO techSO){
        id = techSO.id;
        techName = techSO.techName;
        icon = techSO.icon;
        requiredTechID.AddRange(techSO.requiredTechID);
        cost = techSO.cost;
        daysRequired = techSO.daysRequired;
        state = TechState.Locked;
    }

    public void ReduceResearchDay(){
        daysRequired--;
        if(daysRequired <= 0)
            state = TechState.Completed;
    }

    public bool CheckResourceCost(){
        if(Office.instance.Money < cost.money)
            return false;
        if(Office.instance.Stone < cost.stone)
            return false;
        if(Office.instance.Wood < cost.wood)
            return false;
        return true;
    }

    public void CheckRequiredTech(TechManager techM){
        if(requiredTechID.Count == 0){
            state = TechState.Unlocked;
            return;
        }

        foreach(int i in requiredTechID){
            if(techM.TechSet[i].state != TechState.Completed)
                return;
        }
        state = TechState.Unlocked;
    }
}
