using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Market : MonoBehaviour
{
    [SerializeField] private int _curPriceWheat;
    [SerializeField] private int _curePriceApple;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SellWheat(){
        Office.instance.Money += Office.instance.Wheat * _curPriceWheat;
        Office.instance.Wheat = 0;

        MainUI.instance.UpdateResourceUi();
    }

    public void SellApple(){
        Office.instance.Money += Office.instance.Apple * _curePriceApple;
        Office.instance.Apple = 0;

        MainUI.instance.UpdateResourceUi();
    }

    public void SellAll(){
        SellWheat();
        SellApple();
    }
}
