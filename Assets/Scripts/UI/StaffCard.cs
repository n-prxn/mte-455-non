using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StaffCard : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private Image profilePic;
    [SerializeField] private TMP_Text candidateName;
    [SerializeField] private TMP_Text idText;
    [SerializeField] private TMP_Text wage;

    [SerializeField] private Button HireButton;
    [SerializeField] private Button FireButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateID(int i){
        id = i;
        UpdateIDText(id.ToString());
    }

    public void UpdateIDText(string s){
        idText.text = "ID : " + s;
    }

    public void UpdateProfilePic(Sprite s){
        profilePic.sprite = s;
    }

    public void UpdateWage(int n){
        wage.text = n.ToString();
    }

    public void UpdateProfileName(string s){
        candidateName.text = s;
    }

    public void Hire(){
        bool hired = Office.instance.ToHireStaff(LaborMarket.instance.LaborInMarket[id]);
        if(hired)
        {
            HireButton.gameObject.SetActive(false);
            FireButton.gameObject.SetActive(true);
        }
    }

    public void Fire(){
        bool fired = Office.instance.ToFireStaff(LaborMarket.instance.LaborInMarket[id]);
        if(fired){
            HireButton.gameObject.SetActive(true);
            FireButton.gameObject.SetActive(false);
        }
    }


}
