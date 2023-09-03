using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerController : MonoBehaviour
{
    private Animator anim;
    private Worker worker;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        worker = GetComponent<Worker>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckState();
    }

    private void CheckState(){
        DisableAll();
        switch(worker.State){
            case UnitState.Idle:
                anim.SetBool("isIdle",true);
                break;
            case UnitState.Walk:
            case UnitState.MoveToAttackBuilding:
            case UnitState.MoveToAttackUnit:
            case UnitState.MoveToDeliver:
            case UnitState.MoveToMine:
                anim.SetBool("isWalk",true);
                break;
            case UnitState.Plow:
                anim.SetBool("isPlow",true);
                break;
            case UnitState.Sow:
                anim.SetBool("isSow",true);
                break;
            case UnitState.Water:
                anim.SetBool("isWater",true);
                break;
            case UnitState.Harvest:
                anim.SetBool("isHarvest",true);
                break;
            case UnitState.AttackBuilding:
            case UnitState.AttackUnit:
                anim.SetBool("isAttack",true);
                break;
            case UnitState.Mining:
                anim.SetBool("isMining",true);
                break;
            case UnitState.Die:
                anim.SetBool("isDead",true);
                break;
        }
    }

    private void DisableAll(){
        anim.SetBool("isIdle",false);
        anim.SetBool("isWalk",false);
        anim.SetBool("isPlow",false);
        anim.SetBool("isSow",false);
        anim.SetBool("isWater",false);
        anim.SetBool("isHarvest",false);
        anim.SetBool("isMining",false);
    }
}
