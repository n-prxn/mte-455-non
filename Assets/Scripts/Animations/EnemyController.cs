using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Animator anim;
    private Enemy enemy;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckState();
    }

    private void CheckState()
    {
        DisableAll();
        switch (enemy.State)
        {
            case UnitState.Idle:
                anim.SetBool("isIdle", true);
                break;
            case UnitState.Walk:
            case UnitState.MoveToAttackBuilding:
                anim.SetBool("isWalk", true);
                break;
            case UnitState.AttackBuilding:
            case UnitState.AttackUnit:
                anim.SetBool("isAttack", true);
                break;
            case UnitState.Die:
                anim.SetBool("isDead", true);
                break;
        }
    }

    private void DisableAll()
    {
        anim.SetBool("isIdle", false);
        anim.SetBool("isWalk", false);
        anim.SetBool("isPlow", false);
        anim.SetBool("isSow", false);
        anim.SetBool("isWater", false);
        anim.SetBool("isHarvest", false);
        anim.SetBool("isAttack", false);
    }
}
