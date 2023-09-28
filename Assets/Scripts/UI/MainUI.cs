using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text staffText;
    [SerializeField] private TMP_Text wheatText;
    [SerializeField] private TMP_Text melonText;
    [SerializeField] private TMP_Text stoneText;
    [SerializeField] private TMP_Text woodText;
    [SerializeField] private TMP_Text appleText;
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private TMP_Text warehouseNameText;
    public TMP_Text WarehouseNameText
    {
        get { return warehouseNameText; }
        set { warehouseNameText = value; }
    }
    [SerializeField] private TMP_Text farmNameText;
    public TMP_Text FarmNameText
    {
        get { return farmNameText; }
        set { farmNameText = value; }
    }

    [Header("Panel")]
    public GameObject warehousePanel;
    public GameObject laborMarketPanel;
    public GameObject farmPanel;
    public GameObject techPanel;
    public GameObject buildingPanel;
    public GameObject resourcePanel;
    public GameObject peoplePanel;

    [Header("Tech Buttons")]
    [SerializeField] private Button[] techBtns;
    [SerializeField] private StructureManager structureManager;

    public static MainUI instance;
    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateResourceUi();
        UpdateDayText();
        SetTechBtn();
        UpdateTechBtns();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTechBtns();
    }

    public void UpdateResourceUi()
    {
        moneyText.text = Office.instance.Money.ToString();
        staffText.text = Office.instance.Workers.Count.ToString();
        wheatText.text = Office.instance.Wheat.ToString();
        melonText.text = Office.instance.Melon.ToString();
        stoneText.text = Office.instance.Stone.ToString();
        woodText.text = Office.instance.Wood.ToString();
        appleText.text = Office.instance.Apple.ToString();
    }

    public void UpdateDayText()
    {
        dayText.text = "Day " + GameManager.instance.Day.ToString();
    }

    public void TogglePanel(GameObject panel)
    {
        if (structureManager.IsConstructing || structureManager.IsDemolishing)
            return;

        if (!panel.activeSelf)
            CloseMenuTab();
        panel.SetActive(!panel.activeSelf);
    }

    public void CloseMenuTab()
    {
        if (structureManager.IsConstructing || structureManager.IsDemolishing)
            return;

        buildingPanel.SetActive(false);
        resourcePanel.SetActive(false);
        peoplePanel.SetActive(false);
    }

    public void ToggleLaborPanel()
    {
        if (structureManager.IsConstructing || structureManager.IsDemolishing)
            return;

        if (!laborMarketPanel.activeInHierarchy)
            laborMarketPanel.SetActive(true);
        else
            laborMarketPanel.SetActive(false);
    }

    public void ToggleFarmPanel()
    {
        if (structureManager.IsConstructing || structureManager.IsDemolishing)
            return;

        if (!farmPanel.activeInHierarchy)
            farmPanel.SetActive(true);
        else
            farmPanel.SetActive(false);
    }

    public void ToggleWarehousePanel()
    {
        if (structureManager.IsConstructing || structureManager.IsDemolishing)
            return;

        if (!warehousePanel.activeInHierarchy)
            warehousePanel.SetActive(true);
        else
            warehousePanel.SetActive(false);
    }

    public void ToggleTechPanel()
    {
        if (structureManager.IsConstructing || structureManager.IsDemolishing)
            return;

        if (!techPanel.activeInHierarchy)
            techPanel.SetActive(true);
        else
            techPanel.SetActive(false);
    }

    public void ClickResearchTech(int i)
    {
        if (TechManager.instance.ResearchTech(i))
        {
            UpdateResourceUi();
            techBtns[i].interactable = false;
            techBtns[i].transform.GetChild(1).GetComponent<TMP_Text>().text = "In Progress";
        }
    }

    public void SetTechBtn()
    {
        for (int i = 0; i < techBtns.Length; i++)
        {
            techBtns[i].transform.GetChild(0).GetComponent<TMP_Text>().text = TechManager.instance.TechSet[i].TechName;
        }
    }

    public void UpdateTechBtns()
    {
        for (int i = 0; i < techBtns.Length; i++)
        {
            if (TechManager.instance.CheckTechState(i, TechState.Locked))
            {
                techBtns[i].interactable = false;
                techBtns[i].transform.GetChild(1).GetComponent<TMP_Text>().text = "Locked";
            }
            if (TechManager.instance.CheckTechState(i, TechState.Unlocked))
            {
                techBtns[i].interactable = true;
                techBtns[i].transform.GetChild(1).GetComponent<TMP_Text>().text = "";
            }
            if (TechManager.instance.CheckTechState(i, TechState.InProgress))
            {
                techBtns[i].interactable = false;
                techBtns[i].transform.GetChild(1).GetComponent<TMP_Text>().text = "In Progress";
            }
            if (TechManager.instance.CheckTechState(i, TechState.Completed))
            {
                techBtns[i].interactable = false;
                techBtns[i].transform.GetChild(1).GetComponent<TMP_Text>().text = "Completed";
            }
        }
    }
}
