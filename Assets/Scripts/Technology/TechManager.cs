using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechManager : MonoBehaviour
{
    [SerializeField]
    private List<Technology> techSet = new List<Technology>();
    public List<Technology> TechSet { get => techSet; set => techSet = value; }

    [SerializeField] private TechSO[] techSOs;
    public static TechManager instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        GenTechSetFromSO();
        CheckAllResearch();
    }

    private void GenTechSetFromSO()
    {
        for (int i = 0; i < techSOs.Length; i++)
        {
            Technology tech = new Technology();
            tech.InitData(techSOs[i]);
            techSet.Add(tech);
        }
    }

    public bool CheckTechState(int i, TechState s)
    {
        if (techSet[i].State == s)
            return true;
        else
            return false;
    }

    public bool ResearchTech(int i)
    {
        if (techSet[i].State == TechState.Unlocked)
        {
            if (techSet[i].CheckResourceCost())
            {
                techSet[i].State = TechState.InProgress;

                Office.instance.Money -= techSet[i].Cost.money;
                Office.instance.Stone -= techSet[i].Cost.stone;
                Office.instance.Wood -= techSet[i].Cost.wood;
                return true;
            }
        }
        return false;
    }

    public void CheckAllResearch()
    {
        foreach (Technology t in techSet)
        {
            if (t.State == TechState.Locked)
                t.CheckRequiredTech(this);

            if (t.State == TechState.InProgress)
                t.ReduceResearchDay();
        }
    }

    public float CheckTechBonus(int i)
    {
        float bonus = 0;
        if (techSet[i].State != TechState.Completed)
            return 0;

        switch (i)
        {
            case 1:
                bonus = 0.15f;
                break;
            case 2:
                bonus = 0.25f;
                break;
            case 3:
                bonus = 0.25f;
                break;
            case 4:
                bonus = 0.20f;
                break;
        }
        return bonus;
    }
}
