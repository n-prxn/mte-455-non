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

    [SerializeField] private TMP_Text[] techTexts;
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
        SetTechBtnIcons();
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
        if (!panel.activeSelf)
            CloseMenuTab();
        panel.SetActive(!panel.activeSelf);
    }

    public void CloseMenuTab()
    {
        buildingPanel.SetActive(false);
        resourcePanel.SetActive(false);
        peoplePanel.SetActive(false);
    }

    public void ToggleLaborPanel()
    {
        if (!laborMarketPanel.activeInHierarchy)
            laborMarketPanel.SetActive(true);
        else
            laborMarketPanel.SetActive(false);
    }

    public void ToggleFarmPanel()
    {
        if (!farmPanel.activeInHierarchy)
            farmPanel.SetActive(true);
        else
            farmPanel.SetActive(false);
    }

    public void ToggleWarehousePanel()
    {
        if (!warehousePanel.activeInHierarchy)
            warehousePanel.SetActive(true);
        else
            warehousePanel.SetActive(false);
    }

    public void ToggleTechPanel()
    {
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
            techTexts[i].text = "In Progress";
        }
    }

    public void SetTechBtnIcons()
    {
        for (int i = 0; i < techBtns.Length; i++)
        {
            techBtns[i].image.sprite = TechManager.instance.TechSet[i].Icon;
        }
    }

    public void UpdateTechBtns()
    {
        for (int i = 0; i < techBtns.Length; i++)
        {
            if (TechManager.instance.CheckTechState(i, TechState.Locked))
            {
                techBtns[i].interactable = false;
                techTexts[i].text = "Locked";
            }
            if (TechManager.instance.CheckTechState(i, TechState.Unlocked))
            {
                techBtns[i].interactable = true;
                techTexts[i].text = "";
            }
            if (TechManager.instance.CheckTechState(i, TechState.InProgress))
            {
                techBtns[i].interactable = false;
                techTexts[i].text = "In Progress";
            }
            if (TechManager.instance.CheckTechState(i, TechState.Completed))
            {
                techBtns[i].interactable = false;
                techTexts[i].text = "Completed";
            }
        }
    }
}
