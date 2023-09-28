using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StructureType
{
    road,
    building,
    mine,
    lumber,
    wheat,
    apple,
    melon,
    corn,
    milk
}

public abstract class Structure : MonoBehaviour
{



    [SerializeField] private int id;
    public int ID
    {
        get { return ID; }
        set { ID = value; }
    }
    [SerializeField] private string structureName;
    public string StructureName
    {
        get { return structureName; }
        set { structureName = value; }
    }
    [SerializeField] protected StructureType structureType;
    public StructureType StructureType
    {
        get { return structureType; }
        set { structureType = value; }
    }
    [SerializeField] protected int hp;
    public int HP
    {
        get { return hp; }
        set { hp = value; }
    }

    [SerializeField] private int costToBuild;
    public int CostToBuild
    {
        get { return costToBuild; }
        set { costToBuild = value; }
    }

    [SerializeField] protected bool functional;
    public bool Functional
    {
        get { return functional; }
        set { functional = value; }
    }

    public void TakeDamage(int n)
    {
        hp -= n;
        if (hp <= 0)
            Destroy(gameObject);
    }
}
