using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitStateBubble : MonoBehaviour
{
    public Image stateBubbleImg;
    
    public Sprite miningState;
    public Sprite attackState;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStateChange(UnitState state){
        stateBubbleImg.enabled = true;
        CheckState(state);
    }

    private void CheckState(UnitState state){
        switch(state){
            case UnitState.Mining:
                stateBubbleImg.color = Color.white;
                stateBubbleImg.sprite = miningState;
                break;
            case UnitState.AttackUnit:
            case UnitState.AttackBuilding:
                stateBubbleImg.color = Color.white;
                stateBubbleImg.sprite = attackState;
                break;
            default:
                stateBubbleImg.color = Color.white;
                stateBubbleImg.enabled = false;
                break;
        }
    }
}
