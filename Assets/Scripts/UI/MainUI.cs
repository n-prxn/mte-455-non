using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainUI : MonoBehaviour
{
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text staffText;
    [SerializeField] private TMP_Text wheatText;
    [SerializeField] private TMP_Text melonText;
    [SerializeField] private TMP_Text cornText;
    [SerializeField] private TMP_Text milkText;
    [SerializeField] private TMP_Text appleText;
    [SerializeField] private TMP_Text dayText;

    public GameObject laborMarketPanel;

    public static MainUI instance;

    void Awake(){
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateResourceUi();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateResourceUi(){
        moneyText.text = Office.instance.Money.ToString();
        staffText.text = Office.instance.Workers.Count.ToString();
        wheatText.text = Office.instance.Wheat.ToString();
        melonText.text = Office.instance.Melon.ToString();
        cornText.text = Office.instance.Corn.ToString();
        milkText.text = Office.instance.Milk.ToString();
        appleText.text = Office.instance.Apple.ToString();
    }

    public void ToggleLaborPanel(){
        if(!laborMarketPanel.activeInHierarchy)
            laborMarketPanel.SetActive(true);
        else
            laborMarketPanel.SetActive(false);
    }
}
