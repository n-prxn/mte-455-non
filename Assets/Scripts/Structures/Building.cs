using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Structure
{
    [SerializeField] bool isHousing;
    public bool IsHousing{
        get{return isHousing;}
        set{isHousing = value;}
    }

    [SerializeField] bool isWarehouse;
    public bool IsWarehouse{
        get{return isWarehouse;}
        set{isWarehouse = value;}
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
