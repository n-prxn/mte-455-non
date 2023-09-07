using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Unit
{
    [SerializeField] private LayerMask buildingLayerMask;
    [SerializeField] private LayerMask unitLayerMask;
    [SerializeField] float checkForEnemyRate = 1f;
    // Start is called before the first frame update
    void Start()
    {
        buildingLayerMask = LayerMask.GetMask("Building");
        unitLayerMask = LayerMask.GetMask("Unit");
        InvokeRepeating("CheckForAttack", 0f, checkForEnemyRate);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == targetStructure)
        {
            Structure s = other.gameObject.GetComponent<Structure>();
            if ((s != null) && (s.HP > 0))
                SetUnitState(UnitState.AttackBuilding);
        }
    }

    private void CheckForAttack()
    {
        Building enemyBuilding = FindingTarget.CheckForNearestEnemyBuilding(transform.position, detectRange, buildingLayerMask, "Building");
        Unit enemyUnit = FindingTarget.CheckForNearestEnemyUnit(transform.position, detectRange, unitLayerMask, "Unit");

        if (enemyBuilding != null)
        {
            targetStructure = enemyBuilding.gameObject;
            SetUnitState(UnitState.MoveToAttackBuilding);
        }
        else
        {
            targetStructure = null;
            SetUnitState(UnitState.Idle);

            if (enemyUnit != null)
            {
                targetUnit = enemyUnit.gameObject;
                SetUnitState(UnitState.MoveToAttackUnit);
            }
            else
            {
                targetUnit = null;
                SetUnitState(UnitState.Idle);
            }
        }
    }
}
