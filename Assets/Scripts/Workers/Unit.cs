using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum UnitState
{
    Idle,
    Walk,
    Plow,
    Sow,
    Water,
    Harvest,
    MoveToAttackUnit,
    AttackUnit,
    MoveToAttackBuilding,
    AttackBuilding,
    MoveToMine,
    Mining,
    MoveToDeliver,
    Deliver,
    Die
}


public abstract class Unit : MonoBehaviour
{
    [SerializeField] protected int hp = 100;
    public int HP
    {
        get { return hp; }
        set { hp = value; }
    }

    [SerializeField] protected UnitState state;
    public UnitState State
    {
        get { return state; }
        set { state = value; }
    }

    protected NavMeshAgent navAgent;
    public NavMeshAgent NavAgent
    {
        get { return navAgent; }
        set { navAgent = value; }
    }

    protected float distance;
    [SerializeField] protected GameObject targetStructure;
    public GameObject TargetStructure
    {
        get { return targetStructure; }
        set { targetStructure = value; }
    }

    [SerializeField] protected GameObject targetUnit;
    public GameObject TargetUnit
    {
        get { return targetUnit; }
        set { targetUnit = value; }
    }

    [SerializeField] protected float detectRange = 50f;
    public float DetectRange
    {
        get { return detectRange; }
        set { detectRange = value; }
    }

    [SerializeField] protected float attackRange = 2f;
    public float AttackRange
    {
        get { return attackRange; }
        set { attackRange = value; }
    }

    [SerializeField] protected int attackPower = 5;
    public int AttackPower
    {
        get { return attackPower; }
        set { attackPower = value; }
    }

    [SerializeField] protected float CheckStateTimer = 0f;
    [SerializeField] protected float CheckStateTimeWait = 0.5f;
    [SerializeField] protected GameObject[] tools;
    [SerializeField] protected GameObject weapon;

    void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        CheckStaffState();
    }

    protected void CheckStaffState()
    {
        CheckStateTimer += Time.deltaTime;
        if (CheckStateTimer >= CheckStateTimeWait)
        {
            CheckStateTimer = 0;
            SwitchStaffState();
        }
        //Debug.Log(CheckStateTimer);
    }

    protected void SwitchStaffState()
    {
        switch (state)
        {
            case UnitState.Walk:
                WalkUpdate();
                break;
            case UnitState.MoveToAttackBuilding:
                MoveToAttackBuilding();
                break;
            case UnitState.MoveToAttackUnit:
                MoveToAttackUnit();
                break;
            case UnitState.AttackBuilding:
                AttackBuilding();
                break;
            case UnitState.AttackUnit:
                AttackUnit();
                break;
        }
    }

    protected void WalkUpdate()
    {
        distance = Vector3.Distance(navAgent.destination, transform.position);
        if (distance <= 3f)
        {
            navAgent.isStopped = true;
            state = UnitState.Idle;
        }
    }

    public void SetToWalk(Vector3 dest)
    {
        state = UnitState.Walk;

        navAgent.SetDestination(dest);
        navAgent.isStopped = false;
    }

    protected void MoveToAttackBuilding()
    {
        if (targetStructure == null)
        {
            state = UnitState.Idle;
            navAgent.isStopped = true;
            return;
        }
        else
        {
            navAgent.SetDestination(targetStructure.transform.position);
            navAgent.isStopped = false;
        }

        distance = Vector3.Distance(transform.position, targetStructure.transform.position);

        if (distance <= attackRange)
            state = UnitState.AttackBuilding;
    }

    protected void AttackBuilding()
    {
        EquipWeapon();

        if (navAgent != null)
            navAgent.isStopped = true;

        if (targetStructure != null)
        {
            LookAt(targetStructure.transform.position);
            Building b = targetStructure.GetComponent<Building>();
            b.TakeDamage(attackPower);
        }
    }

    protected void DisableWeapon()
    {
        weapon.SetActive(false);
    }

    protected void EquipWeapon()
    {
        weapon.SetActive(true);
    }

    protected void LookAt(Vector3 pos)
    {
        Vector3 dir = (pos - transform.position).normalized;
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;


        transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    public void TakeDamage(Unit attacker)
    {
        if(gameObject.tag == "Unit")
            CheckSelfDefense(attacker);
            
        hp -= attacker.attackPower;
        if (hp <= 0)
            Destroy(gameObject);
    }

    public void TakeDamage(Turret attacker)
    {
        hp -= attacker.ShootDamage;
        if (hp <= 0)
            Destroy(gameObject);
    }

    protected void MoveToAttackUnit()
    {
        if (targetUnit == null)
        {
            state = UnitState.Idle;
            navAgent.isStopped = true;
            return;
        }
        else
        {
            navAgent.SetDestination(targetUnit.transform.position);
            navAgent.isStopped = false;
        }

        distance = Vector3.Distance(transform.position, targetUnit.transform.position);
        if (distance < attackRange)
        {
            state = UnitState.AttackUnit;
        }
    }

    protected void AttackUnit()
    {
        EquipWeapon();
        if (navAgent != null)
            navAgent.isStopped = true;

        if (targetUnit != null)
        {
            LookAt(targetUnit.transform.position);

            Unit u = targetUnit.GetComponent<Unit>();
            u.TakeDamage(this);
        }
        else
        {
            targetUnit = null;
            state = UnitState.Idle;
        }
    }

    public void CheckSelfDefense(Unit u)
    {
        if (u.gameObject != null)
        {
            targetUnit = u.gameObject;
            state = UnitState.MoveToAttackUnit;
        }
    }
}
