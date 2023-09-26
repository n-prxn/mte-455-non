using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceStructure : Structure
{
    public void Deplete(int n){
        hp -= n;
        if(hp < 0){
            Destroy(gameObject);
        }
    }
}
