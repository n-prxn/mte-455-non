using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurretState
{
    Idle,
    Defending
}

public class Turret : Structure
{
    [SerializeField] private LayerMask unitLayerMask;
    [SerializeField] private GameObject targetUnit;

    [SerializeField] private float detectRange;
    [SerializeField] private float shootRange;
    [SerializeField] private int shootDamage;
    public int ShootDamage
    {
        get { return shootDamage; }
    }

    [SerializeField] TurretState state = TurretState.Idle;
    // Start is called before the first frame update
    void Start()
    {
        unitLayerMask = LayerMask.GetMask("Unit");
        InvokeRepeating("CheckForAttack", 0f, 0.5f);
    }

    private void CheckForAttack()
    {
        Unit enemyUnit = FindingTarget.CheckForNearestEnemyUnit(transform.position, detectRange, unitLayerMask, "Enemy");
        if (enemyUnit != null)
        {
            targetUnit = enemyUnit.gameObject;
            state = TurretState.Defending;
        }
        else
        {
            targetUnit = null;
            state = TurretState.Idle;
        }

        if (state == TurretState.Defending)
        {
            ShootAtEnemy();
        }
    }

    protected void ShootAtEnemy()
    {
        if (targetUnit != null)
        {
            Unit u = targetUnit.GetComponent<Unit>();

            float dist = Vector3.Distance(transform.position, targetUnit.transform.position);
            if (dist <= shootRange)
                u.TakeDamage(this);
        }
        else
        {
            targetUnit = null;
            state = TurretState.Idle;
        }
    }
}
