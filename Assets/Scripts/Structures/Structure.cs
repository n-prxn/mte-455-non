using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StructureType{
    road,
    building,
    wheat,
    apple,
    melon,
    corn,
    milk
}

public abstract class Structure : MonoBehaviour
{
    [SerializeField] protected StructureType structureType;
    public StructureType StructureType{
        get {return structureType;}
        set {structureType = value;}
    }

    [SerializeField] protected bool functional;
    public bool Functional{
        get {return functional;}
        set {functional = value;}
    }

    [SerializeField] private string structureName;
    public string StructureName{
        get {return structureName;}
        set {structureName = value;}
    }

    [SerializeField] protected int hp;
    public int HP{
        get {return hp;}
        set {hp = value;}
    }

    [SerializeField] private int costToBuild;
    public int CostToBuild{
        get {return costToBuild;}
        set {costToBuild = value;}
    }

    [SerializeField] private int id;
    public int ID{
        get {return ID;}
        set {ID = value;}
    }

    // Start is called before the first frame update
    void Start()
    {
        functional = false;
        hp = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int n){
        hp -= n;
        if(hp <= 0)
            Destroy(gameObject);
    }
}
