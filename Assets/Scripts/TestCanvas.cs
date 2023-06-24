using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestCanvas : MonoBehaviour
{
    public Worker worker;
    // Start is called before the first frame update
    public void WorkerIdle(){
        worker.State = UnitState.Idle;
    }

    public void WorkerWalk(){
        worker.State = UnitState.Walk;
    }

    public void WorkerPlow(){
        worker.State = UnitState.Plow;
    }

    public void WorkerSow(){
        worker.State = UnitState.Sow;
    }

    public void WorkerWater(){
        worker.State = UnitState.Water;
    }

    public void WorkerHarvest(){
        worker.State = UnitState.Harvest;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
