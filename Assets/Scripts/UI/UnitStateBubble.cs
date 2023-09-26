using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitStateBubble : MonoBehaviour
{
    public Image stateBubbleImg;

    public Sprite miningState;
    public Sprite plowingState;
    public Sprite sowingState;
    public Sprite wateringState;
    public Sprite harvestingState;
    public Sprite attackState;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnStateChange(UnitState state)
    {
        stateBubbleImg.enabled = true;
        CheckState(state);
    }

    private void CheckState(UnitState state)
    {
        switch (state)
        {
            case UnitState.CollectResource:
                stateBubbleImg.color = Color.white;
                stateBubbleImg.sprite = miningState;
                break;
            case UnitState.AttackUnit:
            case UnitState.AttackBuilding:
                stateBubbleImg.color = Color.white;
                stateBubbleImg.sprite = attackState;
                break;
            case UnitState.Plow:
                stateBubbleImg.color = Color.white;
                stateBubbleImg.sprite = plowingState;
                break;
            case UnitState.Sow:
                stateBubbleImg.color = Color.white;
                stateBubbleImg.sprite = sowingState;
                break;
            case UnitState.Water:
                stateBubbleImg.color = Color.white;
                stateBubbleImg.sprite = wateringState;
                break;
            case UnitState.Harvest:
                stateBubbleImg.color = Color.white;
                stateBubbleImg.sprite = harvestingState;
                break;
            default:
                stateBubbleImg.color = Color.white;
                stateBubbleImg.enabled = false;
                break;
        }
    }
}
