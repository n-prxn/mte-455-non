using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Technology", menuName = "Technology")]
public class TechSO : ScriptableObject
{
    public int id;
    public string techName;
    public Sprite icon;
    public int[] requiredTechID;
    public TechCost cost;
    public int daysRequired;
    [TextArea] public string description;
}
