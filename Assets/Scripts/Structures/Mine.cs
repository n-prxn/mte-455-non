using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] protected int hp;
    public int HP{
        get { return hp; }
        set { hp = value; }
    }

    public void Deplete(int n){
        hp -= n;
        if(hp < 0){
            Destroy(gameObject);
        }
    }
}
