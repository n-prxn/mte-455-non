using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    [SerializeField] private GameObject wheel;
    Structure s;
    // Start is called before the first frame update
    void Start()
    {
        s = GetComponent<Structure>();
    }

    // Update is called once per frame
    void Update()
    {
        if(s == null && !s.Functional)
            return;

        if(wheel != null)
            wheel.transform.Rotate(0f,0f,0.5f);
    }
}
