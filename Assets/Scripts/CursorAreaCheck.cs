using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorAreaCheck : MonoBehaviour
{
    [SerializeField] private GameObject cursor;
    public GameObject Cursor{
        get{return cursor;}
    }

    private GameObject cursorOverObject;
    public GameObject CursorOverObject{
        get{return cursorOverObject;}
    }

    public static CursorAreaCheck instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
    }

    private void OnTriggerExit(Collider other){

    }
}
